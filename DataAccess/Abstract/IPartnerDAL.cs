using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.PartnerDtos;

namespace DataAccess.Abstract
{
    public interface IPartnerDAL : IService
    {
        Task<IDataResult<List<Get>>> GetAll();
        Task<IResult> Create(Create create);
        Task<IResult> Delete(Guid id);
        Task<IResult> Update(Update update); 
    }
}