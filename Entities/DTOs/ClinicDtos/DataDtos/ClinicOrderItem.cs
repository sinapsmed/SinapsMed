using Core.Entities.DTOs;

namespace Entities.DTOs.ClinicDtos.DataDtos
{
    public class ClinicOrderItem : IDto
    {
        public Guid Id { get; set; }
        public bool IsUsed { get; set; }
        public int Count { get; set; }
        public string Code { get; set; }
        public string UserFullName { get; set; }
        public string UserImage { get; set; }
        public string UnikalKey { get; set; }
        public string ItemUnikalKey { get; set; }
        public string AnalysisName { get; set; }
    }
}