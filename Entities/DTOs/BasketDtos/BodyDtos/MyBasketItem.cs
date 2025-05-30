using Core.Entities.DTOs;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.Enums;

namespace Entities.DTOs.BasketDtos.BodyDtos
{
    public class MyBasketItem : IDto
    {
        public Guid Id { get; set; }
        public ItemType Type { get; set; }
        public string Title { get; set; }
        public Guid ServiceId { get; set; }
        public int Count { get; set; }
        public float Amount { get; set; }
        public float Discounted { get; set; }
        public ClinicBasketDetail? ClinicDetail { get; set; }
    }
}