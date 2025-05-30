namespace Entities.DTOs.AnalysisDtos.Analysis
{
    public class Update
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid PartnerId { get; set; }
        public Guid CategoryId { get; set; }
        public double Price { get; set; }
    }
}