using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Helpers;

namespace Entities.Concrete.BlogEntities;

public class BlogCategory : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public List<BlogCategoryLang> Languages { get; set; }
    public List<Blog>? Blogs { get; set; }
}