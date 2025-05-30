using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;

namespace Entities.DTOs.Admin
{
    public class Create : IDto
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfileLogo { get; set; }
        public string? UserName { get; set; }
    }
}