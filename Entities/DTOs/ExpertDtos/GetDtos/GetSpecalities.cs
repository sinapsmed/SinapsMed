using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class GetSpecalities : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}