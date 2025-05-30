using Core.Entities.DTOs;

namespace Entities.DTOs.CuponDtos
{
    public class CuponService : IDto
    {
        public string Id { get; set; }
        public string Label { get; set; }
    }
}