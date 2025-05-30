using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Entities.DTOs;

namespace Entities.Concrete.Forms.Diagnoses
{
    public class Diagnosis : IEntity, IDto
    {
        [Key]
        public Guid Id { get; set; }
        public string ICD10_Code { get; set; }
        public string WHO_Full_Desc { get; set; }
        public ICollection<AnamnezFormDiagnosis> AnamnezFormDiagnoses { get; set; }
    }
}