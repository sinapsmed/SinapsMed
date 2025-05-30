using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.BannerDtos;

namespace Buisness.Abstract
{
    public interface IBannerService : IService
    {
        Task<IResult> Create(Create banner, string userName);
        Task<IResult> Delete(Guid id);
        Task<IDataResult<Update>> UpdateData(Guid id);
        Task<IResult> Update(Update update, string userName);
        Task<IDataResult<List<Get>>> GetAll();
    }
}