using Entities.Enums;

namespace Entities.DTOs.Helpers
{
    public class ReqFrom
    {
        public Guid? RequesterId { get; set; }
        public Superiority Superiority { get; set; }
        public string? UserId { get; set; }
    }
}