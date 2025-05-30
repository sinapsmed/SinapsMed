using Core.Entities.DTOs;

namespace Entities.DTOs.LocationDtos.Common
{
    public class Get : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}