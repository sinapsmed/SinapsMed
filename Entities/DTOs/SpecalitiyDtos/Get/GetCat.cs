using Core.Entities.DTOs;

namespace Entities.DTOs.SpecalitiyDtos.Get
{
    public class GetCat : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}