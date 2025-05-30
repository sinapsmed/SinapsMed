using Core.Utilities.Results.Abstract;
using Entities.DTOs.BasketDtos.BodyDtos;

namespace DataAccess.Abstract
{
    public interface IBasketDAL
    {
        //Read
        Task<IDataResult<List<MyBasketItem>>> MyBasket(string userId, string? cupon);

        //Add, Create
        Task<IResult> AddItem(AddItem item); 

        //Remove, Delete
        Task<IResult> RemoveItem(string userId, Guid itemId);
        Task<IResult> RemoveAll(string userId);

        //Update, Set Count
        Task<IResult> UpdateCount(string userId, IEnumerable<UpdateRange> updates);
    }
}