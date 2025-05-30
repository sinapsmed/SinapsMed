using System.Globalization;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.DTOs.ExpertDtos;
using Entities.DTOs.ExpertDtos.AdminDtos.Get;
using Entities.DTOs.ExpertDtos.BodyDtos;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Entities.Enums.Appointment;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Experts
{
    public class ExpertAdapter : IExpertDAL
    {
        protected readonly AppDbContext _context;
        protected readonly string _culture;
        protected readonly IStringLocalizer<ExpertAdapter> _dalLocalizer;

        public ExpertAdapter(AppDbContext context, IStringLocalizer<ExpertAdapter> dalLocalizer)
        {
            _context = context;
            _culture = CultureInfo.CurrentCulture.Name;
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IResult> AddExpert(Create create)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> AddHoliday(CreateWorkHoliday holiday)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> AddItemToUserBasket(string userId, List<Guid> anlayses)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> AddPause(CreateWorkPause pause)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> AddPeriod(Guid expertId, double price, Guid periodId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> AddServices(Guid expertId, IEnumerable<Guid> serviceIds)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<Entities.DTOs.AppointmentsDtos.Body.ExpertAppointmentDetailed>> AppointmentDetailed(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<Entities.DTOs.AppointmentsDtos.Body.ExpertAppointment>>> Appointments(Guid expertId, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<Entities.DTOs.AppointmentsDtos.Body.ExpertAppointment>>> Appointments(string expertId, AppointmentStatus? appointmentStatus, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteExpertPeriod(Guid expertId, Guid expertPeriodId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteService(Guid expertId, Guid serviceId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<ExpertPeriodGet>>> ExpertPeriods(Guid expertId, Guid? serviceId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<ExpertList>>> GetAll(Guid? serviceId, string search, int page, int limit)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<ExpertDetailedList>>> GetAllDetailed(Guid? serviceId, string search, int page, int limit)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<GetBoosted>>> GetBoosted(int limit, int page)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<List<ExpertWork>>> GetExpertCalendar(Guid expertId, int year, byte month)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<ExpertServiceDto>> Services(Guid id, Guid? serviceId)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> Update(UpdateData update, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<UpdateData>> UpdateData(Guid expertId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UpdateExpertPeriod(ExpertPeriodGet periodGet)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UpdatePassword(Guid expertId, string oldPassword, string newPassword, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UpdateWorkRoutine(WorkRoutineUpdate routineCreate)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<WorkRoutineUpdate>> UpdateWorkRoutineData(Guid expertId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<string>>> WorkingDays(Guid expertId)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<List<string>>> WorkingHours(DateTime date, Guid expertId)
        {
            throw new NotImplementedException();
        }
    }
}