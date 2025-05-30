using Core.Entities.DTOs;

namespace Entities.DTOs.BlogDtos
{
    public class GetAll : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string Preview { get; set; }
        public string ImgUrl { get; set; }
    }
}