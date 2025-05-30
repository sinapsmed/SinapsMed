using Core.Entities.DTOs;

namespace Entities.DTOs.PartnerDtos
{
    public class Get : IDto
    {
        public Guid Id { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }
}