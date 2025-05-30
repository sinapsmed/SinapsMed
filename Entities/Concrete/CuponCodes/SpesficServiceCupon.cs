using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.CuponCodes
{
    public class SpesficServiceCupon
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CuponId { get; set; }
        public Cupon Cupon { get; set; }
        public Guid ServiceId { get; set; }
    }
}