using Core.Entities.DTOs;
using Entities.Enums;

namespace Entities.DTOs.CuponDtos
{
    public class Create : IDto
    {
        public required DateTime Start { get; set; }
        public required DateTime Expired { get; set; }
        public required byte Discount { get; set; }
        public required int UseLimit { get; set; }
        public required int UseLimitForPerUser { get; set; }
        public required CuponType Type { get; set; }
        public IEnumerable<string> SpesficUserIds { get; set; }
        public IEnumerable<Guid> SpesficServiceIds { get; set; }
        public required string Code { get; set; }
    }
}