namespace Entities.Concrete.Services
{
    public class ServicePeriodLang
    {
        public Guid Id { get; set; }
        public ServicePeriod Period { get; set; }
        public Guid PeriodId { get; set; }
        public string Code { get; set; }
        public string PeriodText { get; set; }
    }
}