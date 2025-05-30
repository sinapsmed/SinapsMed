using Core.Entities;
using Entities.Concrete.Analyses;
using Entities.Concrete.Emails;
using Entities.Concrete.Locations;
using Entities.Concrete.OrderEntities;

namespace Entities.Concrete.Clinics
{
    public class Clinic : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnicalKey { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid EmailId { get; set; }
        public WorkSpaceEmail Email { get; set; }
        public Village Village { get; set; }
        public Guid VillageId { get; set; }
        public ICollection<OrderItem>? Orders { get; set; }
        public List<Analysis>? Analyses { get; set; }
    }
}