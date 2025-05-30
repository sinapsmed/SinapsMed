using Core.Entities.DTOs;

namespace Entities.DTOs.AppointmentsDtos.GetData
{
    public class AppointmentSchedule : IDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
    }
}