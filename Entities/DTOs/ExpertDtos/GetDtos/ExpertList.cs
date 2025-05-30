using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos.GetDtos
{
    public class ExpertList : IDto
    {
        public Guid Id { get; set; }
        public string Services { get; set; }
        public string Resume { get; set; }
        public string PhotoUrl { get; set; }
        public string FullName { get; set; }
        public string Seo { get; set; }
        public string Specality { get; set; }
    }
}