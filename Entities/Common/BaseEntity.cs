using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Common
{
    public class BaseEntity : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        virtual public DateTime CreatedAt { get; set; }
        virtual public string CreatedBy { get; set; }
        virtual public DateTime? UpdatedAt { get; set; }
        virtual public string? UpdatedBy { get; set; }
    }
}
