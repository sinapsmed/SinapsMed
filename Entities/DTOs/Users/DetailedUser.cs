using Core.Entities.DTOs;

namespace Entities.DTOs.Users
{
    public class DetailedUser : IDto
    {
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string UnikalId { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string Id { get; set; }
        public DateTime? DateOfBrith { get; set; }
    }
}