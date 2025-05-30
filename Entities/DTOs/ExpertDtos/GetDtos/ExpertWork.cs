using Entities.Enums;

namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class ExpertWork
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public EventType Type { get; set; }
        public string Title { get; set; }
    }
}