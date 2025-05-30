using Core.Entities.DTOs;

namespace Entities.DTOs.BannerDtos
{
    public class LangUpdate : IDto
    { 
        public string Code { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string LinkTitle { get; set; }
    }
}