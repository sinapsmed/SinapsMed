using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;

namespace Entities.DTOs.PartnerDtos
{
    public class Create : IDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
    }
}