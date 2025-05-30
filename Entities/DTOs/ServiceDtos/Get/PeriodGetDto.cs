using Core.Entities.DTOs;

namespace Entities.DTOs.ServiceDtos.Get
{
    public class PeriodGetDto : IDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int Duration { get; set; }
    }
}