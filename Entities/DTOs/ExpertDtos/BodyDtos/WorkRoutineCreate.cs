namespace Entities.DTOs.ExpertDtos.BodyDtos
{
    public class WorkRoutineUpdate
    {
        public Guid ExpertId { get; set; }
        public TimeSpan Interval { get; set; }  
        public TimeSpan Gap { get; set; }
        public List<WeekDayWorkTime> WeekDayWorkTimes { get; set; }
    }
}