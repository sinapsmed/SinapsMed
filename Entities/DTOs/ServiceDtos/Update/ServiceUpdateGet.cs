using Core.Entities.DTOs;
using Entities.Concrete.Services;

namespace Entities.DTOs.ServiceDtos.Update
{
    public class ServiceUpdateGet : IDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<UpdateLanguageGet> Languages { get; set; }
    }
}