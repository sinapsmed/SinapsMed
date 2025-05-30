using Core.Entities;
using Entities.Common;
using Entities.Concrete.Appointments;
using Entities.Concrete.Forms.Diagnoses;
using Entities.Concrete.Forms.Enum;

namespace Entities.Concrete.Forms
{
    public class AnamnezForm : BaseEntity, IEntity
    {
        public Appointment Appointment { get; set; }
        public Guid AppointmentId { get; set; }
        public Cigarette Cigarette { get; set; }
        public Alcohol Alcohol { get; set; }
        public AgeRange AgeRange { get; set; }
        public string Complaints { get; set; }
        public string OtherDiseases { get; set; }
        public string? Allergy { get; set; }
        public string? Medicines { get; set; }
        public string OnlineIndicators { get; set; }
        public string VitalIndicators { get; set; }
        public byte Height { get; set; }
        public int Kq { get; set; }
        public string LaboratoryResult { get; set; }
        public string RadiologicalResult { get; set; }
        public string CheckUpVisitRecords { get; set; }
        public string? CheckUpNotes { get; set; }
        public DateTime NextCheckUpTime { get; set; }
        public string? NextCheckUpNotes { get; set; }
        public ICollection<AnamnezFormDiagnosis> AnamnezFormDiagnoses { get; set; }
    }
}