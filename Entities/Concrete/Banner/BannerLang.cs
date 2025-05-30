using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Concrete.Banner
{
    public class BannerLang : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid BannerId { get; set; }
        public Banner Banner { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string LinkTitle { get; set; }
    }
}