namespace Entities.Concrete.Experts.WorkTimes
{
    public class WrokIntervalHoursData
    {
        public Guid Id { get; set; }
        public WorkRoutineWeekDay WeekDay { get; set; }
        public Guid WeekDayId { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}