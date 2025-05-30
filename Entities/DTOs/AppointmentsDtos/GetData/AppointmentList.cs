using Core.Entities.DTOs;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.Users;
using Entities.Enums.Appointment;

namespace Entities.DTOs.AppointmentsDtos.GetData
{
    public class AppointmentList : IDto
    {
        public PeriodGetDto Service { get; set; }
        public DetailedUser User { get; set; }
        public AppointmentStatus Status { get; set; }
        public ExpertDetail Expert { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public Guid Id { get; set; }
    }
}