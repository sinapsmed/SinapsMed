using Core.Entities.DTOs;

namespace Entities.DTOs.AnalysisDtos.Category
{
    public class GetCats : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}