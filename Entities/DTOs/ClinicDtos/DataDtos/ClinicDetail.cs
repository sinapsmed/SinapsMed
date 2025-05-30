using Core.Entities.DTOs;

namespace Entities.DTOs.ClinicDtos.DataDtos
{
    public class ClinicDetail : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnicalKey { get; set; }
        public string Email { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}