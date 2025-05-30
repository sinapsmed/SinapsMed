using Core.Entities.DTOs;

namespace Entities.DTOs.Helpers
{
    public class CreateOffer : IDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}