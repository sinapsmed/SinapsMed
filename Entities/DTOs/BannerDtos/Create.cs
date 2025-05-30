using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;

namespace Entities.DTOs.BannerDtos
{
    public class Create : IDto
    {
        [Required]
        public string Link { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public List<LangCreate> LanguagesCreate { get; set; }
    }
}