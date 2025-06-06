using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.CuponCodes;
using Entities.Concrete.UserEntities;
using Entities.DTOs.BasketDtos.BodyDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD
{
    public class EFBasketReadDAL : BasketAdapter
    {
        public EFBasketReadDAL(
            AppDbContext context,
            IStringLocalizer<BasketAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }
        public override async Task<IDataResult<List<MyBasketItem>>> MyBasket(string userId, string? cupon)
        {
            var user = await _context.Set<AppUser>()
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                        .ThenInclude(c => c.Analysis)
                            .ThenInclude(c => c.Category)
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                        .ThenInclude(c => c.Clinic)
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                        .ThenInclude(c => c.Appointment)
                            .ThenInclude(c => c.ServicePeriod)
                                .ThenInclude(c => c.ExpertPeriods)
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                        .ThenInclude(c => c.Appointment)
                            .ThenInclude(c => c.ServicePeriod)
                                .ThenInclude(c => c.Service)
                                    .ThenInclude(c => c.Languages)
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                        .ThenInclude(c => c.Clinic)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (user is null)
            {
                return new ErrorDataResult<List<MyBasketItem>>(_dalLocalizer["userNotFound"], HttpStatusCode.NotFound);
            }

            if (user.Basket is null)
            {
                user.Basket = new();
            }

            var data = new List<MyBasketItem>();

            bool cuponUsed = false;
            if (!string.IsNullOrWhiteSpace(cupon))
            {
                var cuponCode = await _context.Set<Cupon>()
                    .Include(c => c.SpesficCuponUsers)
                    .Include(c => c.SpesficServiceCupons)
                    .Include(c => c.UsedCupons)
                    .FirstOrDefaultAsync(c => c.Code == cupon);

                data = user.Basket.Items
                    .Select(BasketSelector.Items(_cultre, new List<Guid>())).ToList();

                if (cuponCode is not null && cuponCode.UsedCupons.Count() < cuponCode.UseLimit && (cuponCode.SpesficCuponUsers.Any(c => c.UserId == userId) || cuponCode.SpesficCuponUsers.Count == 0))
                {

                    //burdanda sual ola biler
                    if (cuponCode.ExpiredAt <= DateTime.UtcNow && cuponCode.StartAt >= DateTime.UtcNow)
                    {
                        return new SuccessDataResult<List<MyBasketItem>>(data, _dalLocalizer["expiredCode"], HttpStatusCode.OK);
                    }

                    if (cuponCode.UsedCupons.Where(c => c.UserId == userId).Count() >= cuponCode.UseLimitForPerUser)
                        return new SuccessDataResult<List<MyBasketItem>>(data, _dalLocalizer["userLimitReached"], HttpStatusCode.OK);

                    data = user.Basket.Items
                        .Select(BasketSelector.Items(_cultre, cuponCode.SpesficServiceCupons.Select(c => c.ServiceId).ToList(), cuponCode.Type, cuponCode.Discount))
                        .ToList();

                    cuponUsed = true;

                    return new SuccessDataResult<List<MyBasketItem>>(data, cuponUsed ? _dalLocalizer["cuponUsed"] : _dalLocalizer["succesUsing"], HttpStatusCode.Accepted);
                }
                else
                {
                    return new SuccessDataResult<List<MyBasketItem>>(data, _dalLocalizer["invalidCode"], HttpStatusCode.OK);
                }
            }
            else
            {
                data = user.Basket.Items
                    .Select(BasketSelector.Items(_cultre, new List<Guid>())).ToList();

                return new SuccessDataResult<List<MyBasketItem>>(data, HttpStatusCode.OK);
            }
        }
    }
}