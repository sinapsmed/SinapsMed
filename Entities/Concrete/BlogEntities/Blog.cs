using Core.Entities;
using Entities.Common;
using Entities.Concrete.Experts;
using Entities.Enums.Blog;

namespace Entities.Concrete.BlogEntities;

public class Blog : BaseEntity, IEntity
{
    public Expert? Expert { get; set; }
    public Guid? ExpertId { get; set; }
    public string ImageUrl { get; set; }
    public Creator Creator { get; set; }
    public int Like { get; set; }
    public List<BlogLang> Languages { get; set; }
    public BlogCategory Category { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
    public Guid CategoryId { get; set; }
    public int View { get; set; }
}