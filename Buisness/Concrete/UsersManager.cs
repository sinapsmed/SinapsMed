using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Users;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.Users;
using Entities.Enums;
using Entities.Enums.Appointment;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class UsersManager : IUsersService
    {
        private readonly UsersServiceFactory _factory;
        private readonly IStringLocalizer<CommonLocalizer> _localizer;

        public UsersManager(UsersServiceFactory factory, IStringLocalizer<CommonLocalizer> localizer)
        {
            _factory = factory;
            _localizer = localizer;
        }

        public async Task<IDataResult<UserAppointmentDetailed>> ApointmentDetail(Guid appointmentId)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.ApointmentDetail(appointmentId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserAppointmentDetailed>(_localizer["ex"], ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetUserAnlysis>>> GetUserAnlysis(string userId, int page)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.GetUserAnlysis(userId, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetUserAnlysis>>(_localizer["ex"], ex);
            }
        }

        public async Task<IDataResult<BaseDto<UserAppointment>>> UserAppointments(string userId, AppointmentStatus? appointmentStatus, int page)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.UserAppointments(userId, appointmentStatus, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<UserAppointment>>(_localizer["ex"], ex);
            }
        }

        public async Task<IDataResult<BaseDto<DetailedUser>>> Users(int page, int limit, string? search)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.Users(page, limit, search);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<DetailedUser>>(_localizer["ex"], ex);
            }
        }
    }
}