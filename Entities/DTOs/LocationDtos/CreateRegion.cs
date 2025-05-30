using System.Threading.Tasks;
using Entities.DTOs.LocationDtos.Common;

namespace Entities.DTOs.LocationDtos
{
    public class CreateRegion : CreateDto
    {
        public Guid CityId { get; set; }
    }
}