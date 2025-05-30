using System.ComponentModel.DataAnnotations;
using Entities.Concrete.BlogEntities;

namespace Entities.DTOs.BlogDtos
{
    public class Create
    {
        [Required]
        public List<BlogLanguagesDto>? Languages { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
    }
}