using Core.Entities;
using Entities.Concrete.Emails;

namespace Entities.Concrete.AccountantEntities
{
    public class Accountant : IEntity
    {
        public Guid Id { get; set; }
        public WorkSpaceEmail Email { get; set; }
        public Guid EmailId { get; set; }
    }
}