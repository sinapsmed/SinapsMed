namespace Entities.DTOs.CuponDtos
{
    public class AnalysisCuponDetail
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsValidAnalysis { get; set; }
    }
}