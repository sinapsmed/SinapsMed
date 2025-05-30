using Core.Entities;
using Entities.Concrete.Experts;

namespace Entities.Concrete.Services
{
    public class Service : IEntity
    {
        public Guid Id { get; set; }
        public ServiceCategory Category { get; set; }
        public Guid CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<ServiceLang> Languages { get; set; }
        public ICollection<Expert> Experts { get; set; }
        public IEnumerable<ServicePeriod> Periods { get; set; }
        public List<Complaint> Complaints { get; set; }
    }
}