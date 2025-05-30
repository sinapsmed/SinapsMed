
namespace Entities.DTOs.ClinicDtos.DataDtos
{
    public class ClinicAnalysisDetail
    {
        public string ItemUnikalKey { get; set; }
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsUsed { get; set; }
        public int Count { get; set; }
        public OrderItemUser User { get; set; }
    }
}