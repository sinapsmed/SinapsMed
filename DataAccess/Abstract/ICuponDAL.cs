using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.CuponDtos;
using Entities.DTOs.CuponDtos.Body;
using Entities.Enums;

namespace DataAccess.Abstract
{
    public interface ICuponDAL : IService
    {
        Task<IResult> Create(Create create);
        //Delete
        Task<IResult> Delete(Guid id);
        //Get 
        Task<IDataResult<BaseDto<CuponService>>> GetServices(CuponServiceBody body);
        Task<IDataResult<BaseDto<CuponService>>> GetUsers(CuponUserBody body);
        Task<IDataResult<BaseDto<Get>>> GetAll(int page, int limit);
        Task<IDataResult<byte>> Discount(string code, string? userName, CuponType type);
        Task<IDataResult<Detail>> GetById(Guid id);
        Task<IDataResult<BaseDto<UsedCupon>>> GetUsedCupons(Guid cuponId, int page);
        Task<IDataResult<Update>> UpdateData(Guid cuponId);
        //Update
        Task<IResult> Update(Update update);
        Task<IResult> UseCupon(string userId, string code, double amount);
    }
}