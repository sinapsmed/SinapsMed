using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;

namespace Entities.DTOs.AnalysisDtos.Analysis
{
    public class Create : IDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }    
        [Required]
        public Guid PartnerId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public double Price { get; set; }
        public Guid ClinicId { get; set; }
    }
}