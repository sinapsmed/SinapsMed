using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.OrderDtos.BodyDtos;
using Entities.DTOs.PaymentDtos;
using Entities.Enums;
using Entities.Enums.Payments;

namespace DataAccess.Abstract
{
    public interface IOrderDAL
    {
        Task<IDataResult<BaseDto<GetAll>>> GetAll(string unikalKey, string userName, Superiority superiority, int page);
        Task<IDataResult<DetailedOrder>> Detailed(Guid id);
        Task<IDataResult<RedirectionDto>> CheckOutOrder(string user, string? cupon);
        Task<IResult> Payment(int paymetId, PaymentStatus status, string? unikalKey = null);
    }
}