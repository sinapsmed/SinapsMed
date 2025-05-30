using Core.Entities.DTOs;

namespace Entities.DTOs.QuestionDtos
{
    public class Get : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}