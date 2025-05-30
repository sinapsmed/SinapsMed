using Core.Entities;
using Entities.Common;

namespace Entities.Concrete.Faq
{
    public class Question : BaseEntity ,IEntity
    {
        public List<QuestionLang>? Languages { get; set; }
    }
}