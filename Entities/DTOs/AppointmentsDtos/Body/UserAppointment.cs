using Core.Entities.DTOs;
using Entities.Enums.Appointment;

namespace Entities.DTOs.AppointmentsDtos.Body
{

    public class UserAppointment : IDto
    {
        public Guid Id { get; set; }
        public string UnikalKey { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string? ExpertPhone { get; set; }
        public virtual string ExpertName { get; set; }
        public virtual string ExpertImageUrl { get; set; }
    }

    public class UserAppointmentDetailed : UserAppointment
    {
        public string? UserNote { get; set; }
        public string ServiceName { get; set; }
        public Guid ServiceId { get; set; }
        public int Duration { get; set; }
        public string UserWhatsApp { get; set; }
        public string MeetLink { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime Date { get; set; }
        public AdditionalUserDto AdditionalUser { get; set; }
        public ICollection<AppointmentAttachmentDto> Attachments { get; set; }
    }

    public class AppointmentAttachmentDto
    {
        public string Attachment { get; set; }
        public string Title { get; set; }
    }

    public class PaymentVerifiedForAppointment
    {
        public Guid Id { get; set; }
        public double Price { get; set; }
    }
}