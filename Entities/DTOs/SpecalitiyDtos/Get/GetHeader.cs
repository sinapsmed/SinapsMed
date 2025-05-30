using Core.Entities.DTOs;

namespace Entities.DTOs.SpecalitiyDtos.Get
{
    public class GetHeader : IDto
    {
        public string Name { get; set; }
        public List<SubHeader> SubRoutes { get; set; }
    }
}