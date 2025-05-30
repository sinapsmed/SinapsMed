namespace Entities.Concrete.Experts.WorkTimes
{
    public class WorkHoliday
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string HolidayTitle { get; set; }
        public bool IsGlobal { get; set; }
        public ICollection<Expert> Experts { get; set; } = new List<Expert>();
    }
}