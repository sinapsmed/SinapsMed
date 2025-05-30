using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;
using Entities.Enums.Appointment;

namespace Entities.DTOs.AppointmentsDtos.Body
{
    public class Create : IDto
    {
        public Guid ServicePeriodId { get; set; }
        [Required]
        public string UserId { get; set; }
        public Guid ExpertId { get; set; }
        public string? UserNote { get; set; }
        public string UserWhatsApp { get; set; }
        public DateTime Date { get; set; }
        public AdditionalUserDto? AdditionalUser { get; set; }
        public AppointmentType AppointmentType { get; set; }
    }
}