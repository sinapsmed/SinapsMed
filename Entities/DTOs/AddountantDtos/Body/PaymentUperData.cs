using Core.Entities;
using Core.Entities.DTOs;

namespace Entities.DTOs.AddountantDtos.Body
{
    public class PaymentUperData<TData> : BaseDto<TData>
        where TData : class, IDto
    {
        public double TotalAmount { get; set; }
    }
}