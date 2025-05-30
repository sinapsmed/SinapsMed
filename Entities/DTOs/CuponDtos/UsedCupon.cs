using Core.Entities.DTOs;

namespace Entities.DTOs.CuponDtos
{
    public class UsedCupon : IDto
    {
        public byte Discount { get; set; }
        public double Amount { get; set; }
        public DateTime UsedAt { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string UnikalKey { get; set; }
    }
}