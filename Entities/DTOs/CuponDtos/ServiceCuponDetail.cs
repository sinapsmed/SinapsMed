namespace Entities.DTOs.CuponDtos
{
    public class ServiceCuponDetail
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public bool IsValidService { get; set; }
    }
}