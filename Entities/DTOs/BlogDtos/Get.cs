using Core.Entities.DTOs;

namespace Entities.DTOs.BlogDtos
{
    public class Get : IDto
    {
        public string Href { get; set; }
        public int CommentCount { get; set; }
        public string CategoryName { get; set; }
        public string Preview { get; set; }
        public Guid CategoryId { get; set; }
        public Guid Id { get; set; }
        public string CreateorRole { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Creator { get; set; }
        public string CreatorImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Like { get; set; }
        public int View { get; set; }
    }
}