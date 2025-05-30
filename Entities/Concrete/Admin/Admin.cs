using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Concrete.Admin
{
    public class Admin : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte WrongPassword { get; set; }
        public string ProfileLogo { get; set; }
    }
}