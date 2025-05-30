using Core.Entities.DTOs;
using Entities.DTOs.ExpertDtos.GetDtos;

namespace Entities.DTOs.ExpertDtos.AdminDtos.Get
{
    public class ExpertDetailedList : ExpertList, IDto
    {
        public string Email { get; set; }
        public string Specality { get; set; }
        public byte Fee { get; set; }
    }
}