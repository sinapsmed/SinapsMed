using Core.Entities.DTOs;
using Entities.Enums;

namespace Entities.DTOs.CuponDtos
{
    public class Get : IDto
    {
        public Guid Id { get; set; }
        public int UseLimit { get; set; }
        public int Used { get; set; }
        public int Discount { get; set; }
        public CuponType Type { get; set; }
        public string Code { get; set; }
        public bool IsExpired { get; set; }
    }
}