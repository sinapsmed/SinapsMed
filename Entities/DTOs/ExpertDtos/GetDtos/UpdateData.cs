using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class UpdateData : IDto
    {
        public Guid ExpertId { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public string Resume { get; set; }
        public bool IsSuspend { get; set; }
        public bool IsBoosted { get; set; }
        public bool IsActive { get; set; }
        public byte Fee { get; set; }
        public string Specality { get; set; }
        public string Email { get; set; }
    }
}