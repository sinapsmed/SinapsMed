using Entities.Enums;

namespace Entities.DTOs.OrderDtos.BodyDtos
{
    public class ItemDetail
    {
        public ItemType Type { get; set; }
        public float Amount { get; set; }
        public int Count { get; set; }
        public string Detail { get; set; }
        public string UnicalKey { get; set; }
        public ClinicDetail? ClinicDetail { get; set; }
    }
}