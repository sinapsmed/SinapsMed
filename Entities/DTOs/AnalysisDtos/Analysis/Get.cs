using Core.Entities.DTOs;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.DTOs.Users;

namespace Entities.DTOs.AnalysisDtos.Analysis
{
    public class Get : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public double Discounted { get; set; }
    }

    public class GetUserAnlysis : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string UnikalKey { get; set; }
        public int Count { get; set; }
        public bool IsVerificated { get; set; }
        public ClinicBasketDetail Clinic { get; set; }
    }

    public class StaffAnalyses : Get, IDto
    {
        public DetailedUser User { get; set; }
        public ClinicBasketDetail Clinic { get; set; }
    }
}