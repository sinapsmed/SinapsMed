using Entities.DTOs.AnalysisDtos.Analysis.Admin;

namespace Entities.DTOs.AnalysisDtos.Analysis
{
    public class Filter : DetailedFilter
    {
        public required Guid ClinicId { get; set; }
    }

}