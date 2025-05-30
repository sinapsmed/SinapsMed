using Core.Entities;

namespace Entities.Concrete.Faq
{
    public class QuestionLang : IEntity
    {
        public Guid Id { get; set; }
        public Question Question { get; set; }  
        public Guid QuestionId { get; set; }
        public string Code { get; set; }    
        public string Title { get; set; }   
        public string Description { get; set; }
    }
}