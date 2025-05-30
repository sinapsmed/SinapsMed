using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Entities.DTOs;

namespace Entities.Concrete.Helpers
{
    public class Offer : IEntity, IDto
    {
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string? SupportAnswer { get; set; }
        public bool IsRead { get; set; } = false;
    }
}