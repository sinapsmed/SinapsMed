using Core.Entities;
using Core.Entities.DTOs;

namespace Entities.DTOs.AddountantDtos.ReturnData
{
    public class AppointmentsUpperData<T> : BaseDto<T>
        where T : class, IDto
    {
        public int CompletedCount { get; set; }
        public double TotalAmount { get; set; }
        public double TotalAmountDiscounted { get; set; }
        public double Fee { get; set; }
    }
    public class AppointmentDtoData : IDto
    {
        public string ExpertName { get; set; }
        public string ExpertImage { get; set; }
        public string ExpertEmail { get; set; }
        public int CompletedCount { get; set; }
        public double TotalAmount { get; set; }
        public double TotalAmountDiscounted { get; set; }
        public double Fee { get; set; }
    }
}