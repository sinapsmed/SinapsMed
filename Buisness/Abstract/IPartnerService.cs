
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.PartnerDtos;

namespace Buisness.Abstract
{
    public interface IPartnerService : IService
    {
        Task<IDataResult<List<Get>>> GetAll();
        Task<IResult> Create(Create create);
        Task<IResult> Delete(Guid id);
        Task<IResult> Update(Update update);
    }
}