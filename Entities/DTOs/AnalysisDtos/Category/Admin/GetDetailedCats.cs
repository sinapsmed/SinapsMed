using Core.Entities.DTOs;

namespace Entities.DTOs.AnalysisDtos.Category.Admin
{
    public class GetDetailedCats : GetCats
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public double DiscountedPercent { get; set; }
        public int AnalysesCount { get; set; }
    }
}