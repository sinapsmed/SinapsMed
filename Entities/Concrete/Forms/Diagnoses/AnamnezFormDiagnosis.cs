using Entities.Enums.Appointment;

namespace Entities.Concrete.Forms.Diagnoses
{
    public class AnamnezFormDiagnosis
    {
        public Guid AnamnezFormId { get; set; }
        public AnamnezForm AnamnezForm { get; set; }

        public Guid DiagnosisId { get; set; }
        public Diagnosis Diagnosis { get; set; }

        public DiagnosisType Type { get; set; }
    }

}