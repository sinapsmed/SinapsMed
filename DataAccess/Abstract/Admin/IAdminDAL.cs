using Core.Utilities.Results.Abstract;
using Entities.DTOs.Admin;
using Entities.DTOs.AuthDtos;
using Entities.Enums;

namespace DataAccess.Abstract.Admin
{
    public interface IAdminDAL
    {
        Task<IResult> AddAdmin(Create create);
        Task<IDataResult<List<Get>>> GetAll();
        Task<IResult> ReloadAdmin(Guid id, string password, Superiority superiority);
        Task<IResult> Login(string email, string password);
    }
}