using Core.Entities;
using Core.Helpers;
using Entities.Common;

namespace Entities.Concrete.Banner
{
    public class Banner : BaseEntity, IEntity
    {
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public List<BannerLang>? Languages { get; set; }
    }
}