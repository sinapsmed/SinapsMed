using Core.Entities.DTOs;

namespace Entities.DTOs.LocationDtos.Clinics
{
    public class GetClinics : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
    }
}