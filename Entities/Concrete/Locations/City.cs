using Core.Entities;
using Entities.Common;

namespace Entities.Concrete.Locations
{
    public class City : LocationBase, IEntity
    {
        public IEnumerable<Region>? Regions { get; set; }
    }
}