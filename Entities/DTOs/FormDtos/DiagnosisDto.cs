using Entities.Enums.Appointment;

namespace Entities.DTOs.FormDtos
{
    public class DiagnosisDto
    {
        public string ICD10_Code { get; set; }
        public string WHO_Full_Desc { get; set; }
        public DiagnosisType Type { get; set; }
    }
}