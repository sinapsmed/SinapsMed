using Core.Entities.DTOs;

namespace Entities.DTOs.ServiceDtos.Create
{
    public class PeriodDto : IDto
    {
        public Guid ServiceId { get; set; }
        public int Duration { get; set; }
        public List<Period> Periods { get; set; }
    }
}