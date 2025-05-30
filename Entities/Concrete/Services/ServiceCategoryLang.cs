namespace Entities.Concrete.Services
{
    public class ServiceCategoryLang
    {
        public Guid Id { get; set; }
        public ServiceCategory Category { get; set; }
        public Guid CategoryId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
    }
}