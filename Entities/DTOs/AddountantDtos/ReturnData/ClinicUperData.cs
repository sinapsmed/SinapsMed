using Core.Entities;
using Core.Entities.DTOs;

namespace Entities.DTOs.AddountantDtos.ReturnData
{
    public class ClinicUperData<T> : BaseDto<T>
        where T : class, IDto
    {
        public int UsedAnalyses { get; set; }
        public double AnalysesPriceDiscounted { get; set; }
        public double AnalysesPrice { get; set; }
        public double AnalysesFee { get; set; }
    }

    public class ClinicDto : IDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public double UsedAnalyses { get; set; }
        public double AnalysesPriceDiscounted { get; set; }
        public double AnalysesPrice { get; set; }
        public double AnalysesFee { get; set; }
    }

}