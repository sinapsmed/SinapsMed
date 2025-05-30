using Core.Entities.DTOs;

namespace Entities.DTOs.SpecalitiyDtos.Get
{
    public class Detail : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Href { get; set; }
        public string CategoryName { get; set; }
    }
}