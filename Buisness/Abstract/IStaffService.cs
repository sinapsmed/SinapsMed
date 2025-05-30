using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.StaffDtos.Body;
using Entities.DTOs.StaffDtos.Return;

namespace Buisness.Abstract
{
    public interface IStaffService
    {
        //Create And Add
        Task<IResult> AddAsync(StaffCreate staff);

        //Read 
        Task<IDataResult<BaseDto<AllStaff>>> AllAsync(int page = 1);

        // Delete
        Task<IResult> DeleteAsync(Guid id);
    }
}