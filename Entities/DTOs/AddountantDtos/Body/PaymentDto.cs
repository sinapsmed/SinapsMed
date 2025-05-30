using Core.Entities.DTOs;

namespace Entities.DTOs.AddountantDtos.Body
{
    public class PaymentDto : IDto
    {
        public int Id { get; set; }
        public string UnikalKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public float Amount { get; set; }
    }
}