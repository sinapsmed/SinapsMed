using Core.Entities.DTOs;

namespace Entities.DTOs.BlogDtos
{
    public class GetCategories : IDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Href { get; set; }
    }
}