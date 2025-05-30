using Core.Entities.DTOs;

namespace Entities.DTOs.BannerDtos
{
    public class Get : IDto
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkTitle { get; set; }

    }
}