using Entities.Enums.Payments;

namespace Entities.DTOs.AddountantDtos.Body
{
    public class DetailedPaymentDto
    {
        public int Id { get; set; }
        public string UnikalKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserUnikalKey { get; set; }
        public PaymentStatus Status { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string? Cupon { get; set; }
    }
}