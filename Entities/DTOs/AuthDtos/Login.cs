using Core.Entities.DTOs;

namespace Entities.DTOs.AuthDtos
{
    public class Login : IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}