using Core.Entities;
using Entities.Concrete.Experts;

namespace Entities.Concrete.Services
{
    public class ServicePeriod : IEntity
    {
        public Guid Id { get; set; }
        public Service Service { get; set; }
        public Guid ServiceId { get; set; }
        public int Duration { get; set; }
        public List<ExpertServicePeriod> ExpertPeriods { get; set; }
        public List<ServicePeriodLang> Languages { get; set; }
    }
}