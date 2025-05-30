using Core.Entities;
using Entities.Common;
using Entities.Concrete.Analyses;
using Entities.Concrete.Locations;

namespace Entities.Concrete.Partners
{
    public class Partner : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string LogoUrl { get; set; }
        public IEnumerable<Village>? Villages { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
    }
}