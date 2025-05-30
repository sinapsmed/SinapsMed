using Entities.Enums.Appointment;

namespace Entities.DTOs.AppointmentsDtos.GetData
{
    public class AppointmentDetail : AppointmentList
    {
        public string? UserNote { get; set; }
        public string UserWhatsApp { get; set; }
        public string MeetLink { get; set; }
        public int Duration { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public IEnumerable<string>? Attachments { get; set; }
        public Guid? AnamnezFormId { get; set; }
    }
}