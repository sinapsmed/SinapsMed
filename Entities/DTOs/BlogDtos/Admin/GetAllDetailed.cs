using Core.Entities.DTOs;

namespace Entities.DTOs.BlogDtos.Admin
{
    public class GetAllDetailed : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}