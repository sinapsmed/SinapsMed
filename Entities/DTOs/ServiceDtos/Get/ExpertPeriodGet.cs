namespace Entities.DTOs.ServiceDtos.Get
{
    public class ExpertPeriodGet : PeriodGetDto
    {
        public Guid ExpertId { get; set; }
        public Guid? PeriodId { get; set; }
        public double? Price { get; set; }
    }
}