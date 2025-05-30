using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Helpers;

namespace Entities.Concrete.BlogEntities;

public class BlogLang : IEntity 
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string SeoUrl { get; set; }
    public string Description { get; set; }
    public string Preview { get; set; }
    public string Code { get; set; }
    public Blog Blog { get; set; }
    public Guid BlogId { get; set; }
}