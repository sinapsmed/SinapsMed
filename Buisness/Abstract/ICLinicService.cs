using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.ClinicDtos.DataDtos;

namespace Buisness.Abstract
{
    public interface ICLinicService
    {
        Task<IResult> CheckAsUsed(Guid clinicId, Guid itemId);
        Task<IDataResult<BaseDto<ClinicOrderItem>>> Orders(Guid? clinicId, int page);
        Task<IDataResult<OrderItemDetail>> ItemDetail(Guid clinicId, Guid? itemId, string? code);
        Task<IDataResult<ClinicDetail>> ClinicDetail(Guid clinicId);
    }
}