using System.ComponentModel.DataAnnotations;
using Entities.Concrete.Services;

namespace Entities.Concrete.Experts
{
    public class ExpertServicePeriod
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ExpertId { get; set; }
        public Expert Expert { get; set; }
        public Guid ServicePeriodId { get; set; }
        public ServicePeriod ServicePeriod { get; set; }
        public double Price { get; set; }
    }
}