using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Orders;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.OrderDtos.BodyDtos;
using Entities.DTOs.PaymentDtos;
using Entities.Enums;
using Entities.Enums.Payments;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class OrderManager(OrderServiceFactory _factory, IStringLocalizer<CommonLocalizer> _localizer) : IOrderService
    {
        public async Task<IDataResult<RedirectionDto>> CheckOutOrder(string user, string? cupon)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await dal.CheckOutOrder(user, cupon);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<RedirectionDto>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<DetailedOrder>> Detailed(Guid id)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.Detailed(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<DetailedOrder>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetAll>>> GetAll(string unikalKey, string userName, Superiority superiority, int page)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.GetAll(unikalKey, userName, superiority, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetAll>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Payment(int paymentId, PaymentStatus status)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await dal.Payment(paymentId, status);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}