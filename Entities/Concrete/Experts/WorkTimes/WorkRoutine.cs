using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.Experts.WorkTimes
{
    public class WorkRoutine
    {
        [Key]
        public Guid Id { get; set; }
        public Expert Expert { get; set; }
        public Guid ExpertId { get; set; }
        public TimeSpan Interval { get; set; }
        public TimeSpan Gap { get; set; }
        public ICollection<WorkRoutineWeekDay> DayOfWeeks { get; set; } = new List<WorkRoutineWeekDay>();
    }
}