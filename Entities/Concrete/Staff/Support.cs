using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Entities.Concrete.Emails;

namespace Entities.Concrete.Staff
{
    public class Support : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid WorkSpaceEmailId { get; set; }
        public WorkSpaceEmail WorkSpaceEmail { get; set; }
        public string FullName { get; set; }
    }
}