using Core.Entities.DTOs;

namespace Entities.DTOs.PaymentDtos
{
    public class RedirectionDto : IDto
    {
        public string RedirectionUrl { get; set; }
        public int PaymentId { get; set; }
    }
}