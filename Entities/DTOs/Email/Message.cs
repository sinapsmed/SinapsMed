using Core.Entities.DTOs;

namespace Entities.DTOs.Email
{
    public class Message : IDto
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Date { get; set; }
        public string User { get; set; }
    }
}