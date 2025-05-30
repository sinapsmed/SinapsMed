using Core.Entities;
using Entities.Common;

namespace Entities.Concrete.Locations
{
    public class Region : LocationBase, IEntity
    {
        public City City { get; set; }
        public Guid CityId { get; set; }
        public IEnumerable<Village>? Villages { get; set; }
    }
}