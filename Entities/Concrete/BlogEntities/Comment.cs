using Core.Entities;
using Entities.Common;
using Entities.Concrete.UserEntities;

namespace Entities.Concrete.BlogEntities
{
    public class Comment : BaseEntity, IEntity
    {
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public List<CommentReply> Replies { get; set; }
    }

}