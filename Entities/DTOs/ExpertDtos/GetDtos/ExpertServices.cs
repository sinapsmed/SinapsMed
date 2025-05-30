namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class ExpertServices
    {
        public ExpertServices()
        {
            SubServices = new();
        }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public List<ExpertSubServices> SubServices { get; set; }
    }
}