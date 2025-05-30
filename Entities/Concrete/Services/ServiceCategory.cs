using Core.Entities;

namespace Entities.Concrete.Services
{
    public class ServiceCategory : IEntity
    {
        public Guid Id { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public ICollection<ServiceCategoryLang> Languages { get; set; }
    }
}