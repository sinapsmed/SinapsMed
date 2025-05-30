using Core.Entities.DTOs;

namespace Entities.DTOs.PartnerDtos
{
    public class DetailedPartner : IDto
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public byte Fee { get; set; }
        public string LogoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}