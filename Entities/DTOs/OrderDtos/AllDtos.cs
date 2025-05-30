using Core.Entities.DTOs;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.Enums;

namespace Entities.DTOs.OrderDtos
{
    public class OrderItemDetailed : IDto
    {
        public ItemType Type { get; set; }
        public string UnikalKey { get; set; }
        public bool IsUsed { get; set; }
        public int Count { get; set; }
        public float Amount { get; set; }
        public ClinicBasketDetail? Clinic { get; set; }
    }

    public class OrderDto
    {
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public ItemType Type { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? AppointmentId { get; set; }
        public string UnikalKey { get; set; }
        public int Count { get; set; }
        public float Amount { get; set; }
        public Guid? ClinicId { get; set; }
        public ClinicDto? Clinic { get; set; }
    }

    public class ClinicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnicalKey { get; set; }
        public List<AnalysisDto> Analyses { get; set; } = new();
    }

    public class AnalysisDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CategoryDto? Category { get; set; }
    }

    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

}