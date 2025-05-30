using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos
{
    public class GetBoosted : IDto
    {
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public string SeoUrl { get; set; }
        public Guid Id { get; set; }
        public IEnumerable<string> Specialties { get; set; }
    }
}