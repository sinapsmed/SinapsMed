using Core.Entities;
using Entities.Common;
using Entities.Concrete.Clinics;
using Entities.Concrete.Partners;

namespace Entities.Concrete.Locations
{
    public class Village : LocationBase, IEntity
    {
        public Region? Region { get; set; }
        public Guid RegionId { get; set; }
        public List<Clinic>? Clinics { get; set; }
        public IEnumerable<Partner>? Partners { get; set; }
    }
}