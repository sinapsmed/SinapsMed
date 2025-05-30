using Core.Entities;
using Entities.Common;
using Entities.Concrete.Experts;
using Entities.Concrete.Forms;
using Entities.Concrete.Services;
using Entities.Concrete.UserEntities;
using Entities.Enums.Appointment;

namespace Entities.Concrete.Appointments
{
    public class Appointment : BaseEntity, IEntity
    {
        public Appointment()
        {
            Status = AppointmentStatus.Pending;
            Attachments = new List<AppointmentAttachment>();
            MeetLink = string.Empty;
        }
        public ServicePeriod ServicePeriod { get; set; }
        public double Price { get; set; }
        public string UnicalKey { get; set; }
        public Guid ServicePeriodId { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public int Number { get; set; }
        public Expert Expert { get; set; }
        public Guid ExpertId { get; set; }
        public string? UserNote { get; set; }
        public string UserWhatsApp { get; set; }
        public string MeetLink { get; set; }
        public AnamnezForm? Form { get; set; }
        public DateTime Date { get; set; }
        public Guid? AdditionalUserId { get; set; }
        public AdditionalUser? AdditionalUser { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public AppointmentStatus Status { get; set; }
        public ICollection<AppointmentAttachment>? Attachments { get; set; }
    }
}