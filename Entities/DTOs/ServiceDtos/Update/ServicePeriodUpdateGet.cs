namespace Entities.DTOs.ServiceDtos.Update
{
    public class ServicePeriodUpdateGet
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public int Duration { get; set; }
        public ICollection<UpdatePeriodLanguageGet> Languages { get; set; }
    }
}