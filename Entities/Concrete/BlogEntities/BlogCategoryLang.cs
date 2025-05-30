using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Concrete.BlogEntities;

public class BlogCategoryLang : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Href { get; set; }
    public BlogCategory Category { get; set; }
    public Guid CategoryId { get; set; }
}