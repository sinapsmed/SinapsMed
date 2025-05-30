using Core.Entities;
using Entities.Concrete.UserEntities;

namespace Entities.Concrete.OrderEntities
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public int Number { get; set; }
        public Payment Payment { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }
}