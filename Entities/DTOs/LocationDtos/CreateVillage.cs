using Entities.DTOs.LocationDtos.Common;

namespace Entities.DTOs.LocationDtos
{
    public class CreateVillage : CreateDto
    {
        public Guid RegionId { get; set; }
        public IEnumerable<Guid>? Partners { get; set; }
    }
}