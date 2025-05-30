using Core.Entities.DTOs;

namespace Entities.DTOs.AccountantDtos
{
    public class Read : IDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}