using Entities.DTOs.LocationDtos.Common;

namespace Entities.DTOs.LocationDtos.Admin
{
    public class VillageDetailed : Get
    {
        public IEnumerable<string> Partners { get; set; }
    }
}