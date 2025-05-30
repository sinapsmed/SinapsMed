namespace Entities.DTOs.AnalysisDtos.Category
{
    public class UpdateCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double DiscountedPercent { get; set; }
    }
}