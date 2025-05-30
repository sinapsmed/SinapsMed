using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;

namespace Entities.DTOs.ExpertDtos
{
    public class Create : IDto
    {
        [EmailAddress]
        public string Email { get; set; } 
        public string Resume { get; set; }
        public string FullName { get; set; } 
        public byte Fee { get; set; }
        public string Specality { get; set; }
        //null ve ya whitespace gonderilse default qoyulacaqg 
        public string? PhotoUrl { get; set; } 
        public List<Guid> ServiceId { get; set; }
    }
}