using Core.Utilities.Results.Abstract;
using Entities.DTOs.Admin;
using Entities.Enums;

namespace Buisness.Abstract.Admin
{
    public interface IAdminService
    {
        Task<IResult> AddAdmin(Create create);
        Task<IResult> ReloadAdmin(Guid id, string password, Superiority superiority);
        Task<IDataResult<List<Get>>> GetAll();
        Task<IResult> Login(string email, string password);
    }
}