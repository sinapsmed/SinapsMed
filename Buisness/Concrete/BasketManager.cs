using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Baskets;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.BasketDtos.BodyDtos;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class BasketManager : IBasketService
    {
        private readonly BasketServiceFactory _fact;
        private readonly IStringLocalizer<CommonLocalizer> _localizer;

        public BasketManager(BasketServiceFactory fact, IStringLocalizer<CommonLocalizer> localizer)
        {
            _fact = fact;
            _localizer = localizer;
        }

        public async Task<IResult> AddItem(AddItem item)
        {
            try
            {
                var _dal = _fact.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddItem(item);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<MyBasketItem>>> MyBasket(string userId, string? cupon)
        {
            try
            {
                var _dal = _fact.GetService(ServiceFactoryType.Read);
                var response = await _dal.MyBasket(userId, cupon);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<MyBasketItem>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> RemoveAll(string userId)
        {
            try
            {
                var _dal = _fact.GetService(ServiceFactoryType.Delete);
                var response = await _dal.RemoveAll(userId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> RemoveItem(string userId, Guid itemId)
        {
            try
            {
                var _dal = _fact.GetService(ServiceFactoryType.Delete);
                var response = await _dal.RemoveItem(userId, itemId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdateCount(string userId, IEnumerable<UpdateRange> updates)
        {
            try
            {
                var _dal = _fact.GetService(ServiceFactoryType.Update);
                var response = await _dal.UpdateCount(userId, updates);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}