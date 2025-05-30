using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.BasketDtos.BodyDtos;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Baskets
{
    public class BasketAdapter : BaseAdapter, IBasketDAL
    {
        protected IStringLocalizer<BasketAdapter> _dalLocalizer;
        public BasketAdapter(AppDbContext context, IStringLocalizer<BasketAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IResult> AddItem(AddItem item)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IDataResult<List<MyBasketItem>>> MyBasket(string userId, string? cupon)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> RemoveAll(string userId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> RemoveItem(string userId, Guid itemId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UpdateCount(string userId, IEnumerable<UpdateRange> updates)
        {
            throw new NotImplementedException();
        }
    }
}