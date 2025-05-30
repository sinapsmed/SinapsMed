using Entities.Enums;

namespace Entities.DTOs.ExpertDtos.BodyDtos
{
    public class WeekDayWorkTime
    {
        public WeekDay WeekDay { get; set; }
        public List<WeekDayHoursIntervalDto> HoursInterval { get; set; }
    }
}