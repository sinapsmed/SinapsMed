using Core.Entities.DTOs;

namespace Entities.DTOs.SpecalitiyDtos.Get
{
    public class GetService : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}