using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos.BodyDtos
{
    public class CreateWorkPause : IDto
    {
        public Guid ExpertId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string? Reason { get; set; }
    }
}