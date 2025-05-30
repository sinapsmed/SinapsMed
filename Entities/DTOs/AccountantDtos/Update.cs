using Entities.DTOs.Helpers;

namespace Entities.DTOs.AccountantDtos
{
    public class Update
    {
        public Guid Id { get; set; }
        public ReqFrom? ReqFrom { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? FullName { get; set; }
    }
}