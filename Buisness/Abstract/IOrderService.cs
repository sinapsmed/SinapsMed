using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.OrderDtos.BodyDtos;
using Entities.DTOs.PaymentDtos;
using Entities.Enums;
using Entities.Enums.Payments;

namespace Buisness.Abstract
{
    public interface IOrderService
    {
        Task<IDataResult<BaseDto<GetAll>>> GetAll(string unikalKey, string userName, Superiority superiority, int page);
        Task<IDataResult<DetailedOrder>> Detailed(Guid id);
        Task<IDataResult<RedirectionDto>> CheckOutOrder(string user, string? cupon);
        Task<IResult> Payment(int paymentId, PaymentStatus status);
    }
}