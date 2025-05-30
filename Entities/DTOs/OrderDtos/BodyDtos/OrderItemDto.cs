using Core.Entities.DTOs;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.Enums;

namespace Entities.DTOs.OrderDtos.BodyDtos
{
    public class OrderItemDto : IDto
    {
        public Guid Id { get; set; }
        public ItemType Type { get; set; }
        public string UnikalKey { get; set; }
        public bool IsUsed { get; set; }
        public int Count { get; set; }
        public float Amount { get; set; }
        public string Title { get; set; }
        public ClinicBasketDetail? ClinicDetail { get; set; }
    }
}