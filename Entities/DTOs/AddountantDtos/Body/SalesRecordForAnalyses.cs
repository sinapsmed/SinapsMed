namespace Entities.DTOs.AddountantDtos.Body
{
    public class SalesRecordForAnalyses
    {
        public SalesRecord SalesRecord { get; set; }
        public int SoldAnalyses { get; set; }
        public ICollection<PartnerSalary>? PartnerSalaries { get; set; }
    }
}