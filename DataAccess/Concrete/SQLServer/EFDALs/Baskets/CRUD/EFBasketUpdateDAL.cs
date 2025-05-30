using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.BasketDtos.BodyDtos;
using Entities.DTOs.OrderDtos.BodyDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD
{
    public class EFBasketUpdateDAL : BasketAdapter
    {
        private readonly EFBasketDeleteDAL _dal;
        public EFBasketUpdateDAL(
            AppDbContext context,
            IStringLocalizer<BasketAdapter> dalLocalizer,
            EFBasketDeleteDAL dal) : base(context, dalLocalizer)
        {
            _dal = dal;
        }

        public override async Task<IResult> UpdateCount(string userId, IEnumerable<UpdateRange> updates)
        {
            var user = await _context.Users
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (user is null)
                return new ErrorResult(_dalLocalizer["userNotFound"], HttpStatusCode.NotFound);

            if (user.Basket is null)
                user.Basket = new();

            foreach (var item in updates)
            {
                if (item.NewCount < 0)
                    return new ErrorResult(_dalLocalizer["invalidCount"], HttpStatusCode.BadRequest);

                if (!user.Basket.Items.Any(c => c.Id == item.ItemId))
                    return new ErrorResult(_dalLocalizer["itemNotFound"], HttpStatusCode.NotFound);

                if (item.NewCount == 0)
                    return await _dal.RemoveItem(userId, item.ItemId);

                var basketItem = user.Basket.Items.First(c => c.Id == item.ItemId);
                if (basketItem is null)
                    continue;
                basketItem.Count = item.NewCount;
            }

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
    }
}