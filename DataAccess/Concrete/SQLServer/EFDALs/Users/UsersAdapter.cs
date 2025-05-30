using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.Users;
using Entities.Enums.Appointment;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Users
{
    public class UsersAdapter : BaseAdapter, IUsersDAL
    {
        protected readonly IStringLocalizer<UsersAdapter> _dalLocalizer;
        public UsersAdapter(AppDbContext context, IStringLocalizer<UsersAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IDataResult<UserAppointmentDetailed>> ApointmentDetail(Guid appointmentId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<GetUserAnlysis>>> GetUserAnlysis(string userId, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<UserAppointment>>> UserAppointments(string userId, AppointmentStatus? appointmentStatus, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<DetailedUser>>> Users(int page, int limit, string? search)
        {
            throw new SystemNotWorkingException();
        }
    }
}