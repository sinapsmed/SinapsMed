using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.StaffDtos.Body;
using Entities.DTOs.StaffDtos.Return;

namespace DataAccess.Abstract
{
    public interface IStaffDAL
    {
        //Create And Add
        Task<IResult> AddAsync(StaffCreate staff);

        //Read 
        Task<IDataResult<BaseDto<AllStaff>>> AllAsync(int page);
        Task<IDataResult<BaseDto<AppointmentList>>> Appointments(int page);
        Task<IDataResult<AppointmentStaffDto>> AppointmentDetail();
        Task<IDataResult<BaseDto<StaffAnalyses>>> Analyses(string userId,int page);

        // Delete
        Task<IResult> DeleteAsync(Guid id);

    }
}