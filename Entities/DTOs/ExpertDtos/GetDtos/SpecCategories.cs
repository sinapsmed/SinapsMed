using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class SpecCategories : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}