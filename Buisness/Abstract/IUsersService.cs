using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.Users;
using Entities.Enums.Appointment;

namespace Buisness.Abstract
{
    public interface IUsersService
    {
        Task<IDataResult<BaseDto<DetailedUser>>> Users(int page, int limit, string? search);
        Task<IDataResult<BaseDto<UserAppointment>>> UserAppointments(string userId, AppointmentStatus? appointmentStatus, int page);
        Task<IDataResult<UserAppointmentDetailed>> ApointmentDetail(Guid appointmentId);
        Task<IDataResult<BaseDto<GetUserAnlysis>>> GetUserAnlysis(string userId, int page);
    }
}