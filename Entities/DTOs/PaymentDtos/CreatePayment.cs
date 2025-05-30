namespace Entities.DTOs.PaymentDtos
{
    public class CreatePayment
    {
        public string UserId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public Guid BasketId { get; set; }
    }
}