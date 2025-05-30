using Core.Entities.DTOs;
using Entities.Enums;

namespace Entities.DTOs.BasketDtos.BodyDtos
{
    public class AddItem : IDto
    {
        public string UserId { get; set; }
        public ItemType Type { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? AppointmentId { get; set; }
        public int Count { get; set; }
    }
}