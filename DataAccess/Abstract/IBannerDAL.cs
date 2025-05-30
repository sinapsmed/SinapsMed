using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.BannerDtos;

namespace DataAccess.Abstract
{
    public interface IBannerDAL : IService
    {
        Task<IDataResult<Update>> UpdateData(Guid id);
        Task<IResult> Create(Create banner, string userName);
        Task<IResult> Delete(Guid id);
        Task<IResult> Update(Update update, string userName);
        Task<IDataResult<List<Get>>> GetAll();
    }
}