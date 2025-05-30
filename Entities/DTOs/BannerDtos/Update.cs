using Core.Entities.DTOs;

namespace Entities.DTOs.BannerDtos
{
    public class Update : IDto
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public List<LangUpdate> Languages { get; set; }
    }
}