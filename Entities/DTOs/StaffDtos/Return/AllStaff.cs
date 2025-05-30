using Core.Entities.DTOs;

namespace Entities.DTOs.StaffDtos.Return
{
    public class AllStaff : IDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}