using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos.BodyDtos
{
    public class CreateWorkHoliday : IDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string HolidayTitle { get; set; }
        public bool IsGlobal { get; set; }
        public ICollection<Guid>? Experts { get; set; } = new List<Guid>();
    }
}