namespace Entities.DTOs.AnalysisDtos.Analysis.Admin
{
    public class DetailedGet : Get
    {
        public Guid PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
    }
}