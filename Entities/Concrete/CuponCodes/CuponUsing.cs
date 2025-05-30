using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Entities.Concrete.UserEntities;

namespace Entities.Concrete.CuponCodes
{
    public class CuponUsing : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Cupon Cupon { get; set; }
        public Guid CuponId { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public DateTime UsedAt { get; set; }
        public double Amount { get; set; }
    }
}