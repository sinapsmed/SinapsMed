using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs.BlogDtos
{
    public class BlogLanguagesDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum Length Is 100 character")]
        public string Preview { get; set; }
        public string Title { get; set; }
    }
}