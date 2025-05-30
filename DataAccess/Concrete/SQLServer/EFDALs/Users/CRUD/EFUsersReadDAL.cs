using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Appointments;
using Entities.Concrete.OrderEntities;
using Entities.Concrete.UserEntities;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.Users;
using Entities.Enums;
using Entities.Enums.Appointment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Users.CRUD
{
    public class EFUsersReadDAL : UsersAdapter
    {
        private readonly IRepositoryBase<AppUser, DetailedUser, AppDbContext> _users;
        private readonly IRepositoryBase<Appointment, UserAppointment, AppDbContext> _appointments;
        private readonly IRepositoryBase<OrderItem, GetUserAnlysis, AppDbContext> _analysis;
        public EFUsersReadDAL(
            AppDbContext context, IStringLocalizer<UsersAdapter> dalLocalizer,
            IRepositoryBase<AppUser, DetailedUser, AppDbContext> users,
            IRepositoryBase<Appointment, UserAppointment, AppDbContext> appointments, IRepositoryBase<OrderItem, GetUserAnlysis, AppDbContext> analysis) : base(context, dalLocalizer)
        {
            _users = users;
            _appointments = appointments;
            _analysis = analysis;
        }

        public override async Task<IDataResult<BaseDto<DetailedUser>>> Users(int page, int limit, string? search)
        {
            var users = _context.Users.AsQueryable();
            search = search?.ToLower();

            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(c => c.FullName.ToLower().Contains(search) || c.UnicalKey.ToLower().Contains(search) || c.Email.ToLower().Contains(search));
            }

            DtoFilter<AppUser, DetailedUser> filter = new()
            {
                Limit = limit,
                Page = page,
                Selector = UsersSelector.Users()
            };

            return await _users.GetAllAsync(users, filter);
        }

        public override async Task<IDataResult<BaseDto<UserAppointment>>> UserAppointments(string userId, AppointmentStatus? appointmentStatus, int page)
        {
            var querry = _context.Set<Appointment>()
                .Include(c => c.Expert)
                .Where(c => c.UserId == userId)
                .AsQueryable();

            if (appointmentStatus.HasValue)
            {
                querry = querry.Where(c => c.Status == appointmentStatus);
            }

            var filter = new DtoFilter<Appointment, UserAppointment>
            {
                Limit = 10,
                Page = page,
                Selector = UsersSelector.UserAppointments()
            };

            return await _appointments.GetAllAsync(querry, filter);
        }

        public override async Task<IDataResult<UserAppointmentDetailed>> ApointmentDetail(Guid appointmentId)
        {
            var appointment = await _context.Set<Appointment>()
                .Include(c => c.Expert)
                .Include(c => c.ServicePeriod)
                    .ThenInclude(c => c.Service)
                        .ThenInclude(c => c.Languages)
                .Include(c => c.AdditionalUser)
                .Include(c => c.Attachments)
                .OrderBy(c => c.CreatedAt)
                .FirstOrDefaultAsync(c => c.Id == appointmentId);

            if (appointment == null)
                return new ErrorDataResult<UserAppointmentDetailed>(_dalLocalizer["appointmentNotFound"]);

            var result = UsersSelector.UserAppointmentDetailed(appointment, _cultre);

            return new SuccessDataResult<UserAppointmentDetailed>(result);
        }

        public override async Task<IDataResult<BaseDto<GetUserAnlysis>>> GetUserAnlysis(string userId, int page)
        {
            var orderItems = _context.Set<OrderItem>()
                .Include(c => c.Order)
                .Include(c => c.Clinic)
                .Where(c => c.Order.UserId == userId && c.Type == ItemType.Analysis)
                .OrderBy(c => c.Order.CreatedAt)
                .AsQueryable();

            var filter = new DtoFilter<OrderItem, GetUserAnlysis>
            {
                Limit = 10,
                Page = page,
                Selector = UsersSelector.UserAnalysis()
            };

            return await _analysis.GetAllAsync(orderItems, filter);
        }

    }
}