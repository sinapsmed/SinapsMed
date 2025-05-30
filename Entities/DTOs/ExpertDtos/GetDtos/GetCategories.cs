using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class GetCategories : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<GetSpecalities> Specalities { get; set; }
    }
}