using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Concrete.Emails
{
    public class WorkSpaceEmail : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Title { get; set; }
        public string PasswordHash { get; set; }
    }
}