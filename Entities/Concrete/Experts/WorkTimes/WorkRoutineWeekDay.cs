using System.ComponentModel.DataAnnotations;
using Entities.Enums;

namespace Entities.Concrete.Experts.WorkTimes
{
    public class WorkRoutineWeekDay
    {
        [Key]
        public Guid Id { get; set; }
        public WorkRoutine WorkRoutine { get; set; }
        public Guid WorkRoutineId { get; set; }
        public WeekDay WeekDay { get; set; }
        public ICollection<WrokIntervalHoursData> IntervalHours { get; set; } = new List<WrokIntervalHoursData>();
    }
}