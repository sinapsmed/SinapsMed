using Entities.Concrete.Emails;

namespace Entities.Common
{
    public interface IHaveWorkSpaceEmail
    {
        Guid WorkSpaceEmailId { get; set; }
        WorkSpaceEmail WorkSpaceEmail { get; set; }
    }
}