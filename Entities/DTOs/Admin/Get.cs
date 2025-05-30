using Core.Entities.DTOs;

namespace Entities.DTOs.Admin
{
    public class Get : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte WrongPassword { get; set; }
        public string Image { get; set; }
    }
}