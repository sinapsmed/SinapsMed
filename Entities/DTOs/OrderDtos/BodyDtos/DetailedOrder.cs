using Core.Entities.DTOs;

namespace Entities.DTOs.OrderDtos.BodyDtos
{
    public class DetailedOrder : IDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Number { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
    }
}