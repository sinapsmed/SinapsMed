using Core.Entities.DTOs;

namespace Entities.DTOs.QuestionDtos
{
    public class Create : IDto
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}