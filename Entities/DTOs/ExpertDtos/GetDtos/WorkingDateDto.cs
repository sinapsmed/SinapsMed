using Entities.Enums;

namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class WorkingDateDto
    {
        public WeekDay WeekDay { get; set; }
        public ICollection<WorkIntervalHours> IntervalHours { get; set; }
    }
}