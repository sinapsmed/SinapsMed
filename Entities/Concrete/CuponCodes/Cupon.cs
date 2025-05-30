using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Entities.Enums;

namespace Entities.Concrete.CuponCodes
{
    public class Cupon : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Code { get; set; }
        public byte Discount { get; set; }
        public int UseLimit { get; set; }
        public CuponType Type { get; set; }
        public int UseLimitForPerUser { get; set; }
        public ICollection<CuponUsing>? UsedCupons { get; set; } = new List<CuponUsing>();
        public ICollection<SpesficCuponUser>? SpesficCuponUsers { get; set; } = new List<SpesficCuponUser>();
        public ICollection<SpesficServiceCupon>? SpesficServiceCupons { get; set; } = new List<SpesficServiceCupon>();
    }
}