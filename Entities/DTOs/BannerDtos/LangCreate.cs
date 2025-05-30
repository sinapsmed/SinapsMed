using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;

namespace Entities.DTOs.BannerDtos
{
    public class LangCreate : IDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        public string LinkTitle { get; set; }
    }
}