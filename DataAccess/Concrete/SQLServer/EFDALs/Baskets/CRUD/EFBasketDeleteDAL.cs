using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.BasketEntities;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD
{
    public class EFBasketDeleteDAL : BasketAdapter
    {
        public EFBasketDeleteDAL(
            AppDbContext context,
            IStringLocalizer<BasketAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }

        public override async Task<IResult> RemoveItem(string userId, Guid itemId)
        {
            var user = await _context.Users
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (user is null)
                return new ErrorResult(_dalLocalizer["userNotFound"], HttpStatusCode.NotFound);

            if (user.Basket is null)
                user.Basket = new();

            if (!user.Basket.Items.Any(c => c.Id == itemId))
                return new ErrorResult(_dalLocalizer["itemNotFound"], HttpStatusCode.NotFound);

            user.Basket.Items.Remove(user.Basket.Items.First(c => c.Id == itemId));

            await _context.SaveChangesAsync();
            return new SuccessResult();
        }

        [Queue("high")]
        public override async Task<IResult> RemoveAll(string userId)
        {
            var user = await _context.Users
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (user is null)
                return new ErrorResult(_dalLocalizer["userNotFound"], HttpStatusCode.NotFound);

            if (user.Basket is null)
                user.Basket = new();

            user.Basket.Items = new List<BasketItem>();

            await _context.SaveChangesAsync();
            return new SuccessResult();
        }
    }
}