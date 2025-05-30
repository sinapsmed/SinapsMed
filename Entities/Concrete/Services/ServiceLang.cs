namespace Entities.Concrete.Services
{
    public class ServiceLang
    {
        public Guid Id { get; set; }
        public Service Service { get; set; }
        public Guid ServiceId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
    }
}