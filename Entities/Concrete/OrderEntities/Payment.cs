using Core.Entities;
using Entities.Concrete.UserEntities;
using Entities.Enums.Payments;

namespace Entities.Concrete.OrderEntities
{
    public class Payment : IEntity
    {
        public int Id { get; set; }
        public string UnikalKey { get; set; }
        public long PaymentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public PaymentStatus Status { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string? Cupon { get; set; }
        public Order? Order { get; set; }
        public Guid? OrderId { get; set; }
    }
}