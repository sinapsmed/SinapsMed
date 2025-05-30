using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.Helpers;
using Entities.DTOs.Helpers;

namespace Buisness.Abstract
{
    public interface ICustomService : IService
    {
        Task<IResult> Contact(CreateOffer offer);
        Task<IDataResult<BaseDto<Offer>>> GetAll(int page, int limit);
        Task<IResult> Delete(Guid id);
        Task<IResult> MarkAsRead(Guid id);
        Task<IResult> Reply(Guid id, string message);
    }
}