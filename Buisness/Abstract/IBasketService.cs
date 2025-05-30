using Core.Utilities.Results.Abstract;
using Entities.DTOs.BasketDtos.BodyDtos;

namespace Buisness.Abstract
{
    public interface IBasketService
    {
        //Read
        Task<IDataResult<List<MyBasketItem>>> MyBasket(string userId, string? cupon);

        //Add, Create
        Task<IResult> AddItem(AddItem item); 
        //Remove, Delete
        Task<IResult> RemoveAll(string userId);
        Task<IResult> RemoveItem(string userId, Guid itemId);

        //Update, Set Count
        Task<IResult> UpdateCount(string userId, IEnumerable<UpdateRange> updates);

    }
}