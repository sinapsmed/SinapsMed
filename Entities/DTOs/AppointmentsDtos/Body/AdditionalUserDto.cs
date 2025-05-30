using Core.Entities.DTOs;

namespace Entities.DTOs.AppointmentsDtos.Body
{
    public class AdditionalUserDto : IDto
    {
        public string FullName { get; set; }
        public DateTime DateOfBrith { get; set; }
        public bool Gender { get; set; }
    }
}