using Core.Entities.DTOs;
using Entities.Enums.Appointment;

namespace Entities.DTOs.AppointmentsDtos.Body
{
    public class ExpertAppointment : IDto
    {
        public Guid Id { get; set; }
        public string UnikalKey { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string UserFullName { get; set; }
        public string UserPhotoUrl { get; set; }
    }

    public class ExpertAppointmentDetailed : ExpertAppointment
    {
        public int Number { get; set; }
        public string? UserNote { get; set; }
        public string UserWhatsApp { get; set; }
        public string MeetLink { get; set; }
        public string UnikalKey { get; set; }
        public ICollection<AppointmentAttachmentDto>? Attachments { get; set; }
        public AdditionalUserDto? AdditionalUserDto { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public AppointmentStatus Status { get; set; }
    }

}