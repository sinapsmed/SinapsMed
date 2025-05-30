using System.ComponentModel.DataAnnotations;
using Entities.Concrete.UserEntities;

namespace Entities.Concrete.CuponCodes
{
    public class SpesficCuponUser
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CuponId { get; set; }
        public Cupon Cupon { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
    }
}