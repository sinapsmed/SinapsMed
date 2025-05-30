using Core.Entities.DTOs;

namespace Entities.DTOs.AnalysisDtos.Category
{
    public class CreateCategory : IDto
    {
        public string Name { get; set; }
        public double DiscountedPercent { get; set; }
    }
}