
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Concrete.Experts.WorkTimes
{
    public class WorkPause : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ExpertId { get; set; }
        public Expert Expert { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string? Reason { get; set; }
    }
}