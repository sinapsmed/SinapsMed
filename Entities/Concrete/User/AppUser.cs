using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Entities.Concrete.Appointments;
using Entities.Concrete.BasketEntities;
using Entities.Concrete.OrderEntities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete.UserEntities
{
    public class AppUser : IdentityUser, IEntity
    {
        public string ImageUrl { get; set; }
        public DateTime? DateOfBrith { get; set; }
        public string UnicalKey { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        [MaxLength(100)]
        public string? Country { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        public Basket? Basket { get; set; }
        public List<Appointment>? Appointments { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}