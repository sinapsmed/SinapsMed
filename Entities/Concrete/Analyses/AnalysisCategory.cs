using Core.Entities;

namespace Entities.Concrete.Analyses
{
    public class AnalysisCategory : IEntity
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public double DiscountedPercent { get; set; }
        public IEnumerable<Analysis>? Analyses { get; set; }
    }
}