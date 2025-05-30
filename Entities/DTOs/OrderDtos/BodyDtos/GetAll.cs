using Core.Entities.DTOs;

namespace Entities.DTOs.OrderDtos.BodyDtos
{
    public class GetAll : IDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public float TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}