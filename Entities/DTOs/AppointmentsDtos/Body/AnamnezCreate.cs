using Core.Entities.DTOs;
using Entities.Concrete.Forms.Enum;
using Entities.Enums.Appointment;

namespace Entities.DTOs.AppointmentsDtos.Body
{
    public class AnamnezCreate : IDto
    {
        public Guid AppointmentId { get; set; }
        public Cigarette Cigarette { get; set; }
        public Alcohol Alcohol { get; set; }
        public AgeRange AgeRange { get; set; }
        public string OtherDiseases { get; set; }
        public string Complaints { get; set; }
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
        public ICollection<AnamnezFormDiagnosisDto> AnamnezFormDiagnoses { get; set; }
    }

    public class AnamnezFormDiagnosisDto
    {
        public Guid DiagnosisId { get; set; }
        public DiagnosisType Type { get; set; }
    }
}