using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.Appointments
{
    public class AppointmentAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public string Attachment { get; set; }
        public string Title { get; set; }
        public Appointment Appointment { get; set; }
        public Guid AppointmentId { get; set; }
    }
}