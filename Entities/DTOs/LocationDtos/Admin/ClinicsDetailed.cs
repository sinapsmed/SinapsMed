using Core.Entities.DTOs;

namespace Entities.DTOs.LocationDtos.Admin
{
    public class ClinicsDetailed : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string UnikalKey { get; set; }
    }
}