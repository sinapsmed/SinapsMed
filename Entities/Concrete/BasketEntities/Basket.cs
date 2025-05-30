using Core.Entities;
using Entities.Concrete.UserEntities;

namespace Entities.Concrete.BasketEntities
{
    public class Basket : IEntity
    {
        public Basket()
        {
            Items = new List<BasketItem>();
        }
        public Guid Id { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public ICollection<BasketItem> Items { get; set; }
    }
}