using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Experts;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.ExpertDtos;
using Entities.DTOs.ExpertDtos.AdminDtos.Get;
using Entities.DTOs.ExpertDtos.BodyDtos;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class ExpertManager : Manager, IExpertService
    {
        private readonly IStringLocalizer<Validator> _localizer;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _axs;
        private readonly ExpertServiceFactory _factory;

        public ExpertManager(IStringLocalizer<Validator> localizer, ExpertServiceFactory factory, Microsoft.AspNetCore.Http.IHttpContextAccessor axs)
        {
            _localizer = localizer;
            _factory = factory;
            _axs = axs;
        }

        public async Task<IResult> AddExpert(Create create)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                if (string.IsNullOrWhiteSpace(create.PhotoUrl))
                {
                    var requestContext = _axs?.HttpContext?.Request;
                    string scheme = requestContext?.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? requestContext?.Scheme;

                    string defaultProfilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "Default", "user.png");
                    create.PhotoUrl = defaultProfilePath;
                }
                var response = await _dal.AddExpert(create);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddPeriod(Guid expertId, double price, Guid periodId)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await dal.AddPeriod(expertId, price, periodId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddServices(Guid expertId, IEnumerable<Guid> serviceIds)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await dal.AddServices(expertId, serviceIds);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddHoliday(CreateWorkHoliday holiday)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddHoliday(holiday);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<ExpertList>>> GetAll(Guid? serviceId, string search, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetAll(serviceId, search, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<ExpertList>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<ExpertDetailedList>>> GetAllDetailed(Guid? serviceId, string search, int page, int limit)
        {

            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetAllDetailed(serviceId, search, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<ExpertDetailedList>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetBoosted>>> GetBoosted(int limit, int page)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetBoosted(limit, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetBoosted>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<ExpertServiceDto>> Services(Guid id, Guid? serviceId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.Services(id, serviceId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ExpertServiceDto>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdateWorkRoutine(WorkRoutineUpdate routineCreate)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.UpdateWorkRoutine(routineCreate);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ExpertServiceDto>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddPause(CreateWorkPause pause)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddPause(pause);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<string>>> WorkingDays(Guid expertId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.WorkingDays(expertId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<string>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<string>>> WorkingHours(DateTime date, Guid expertId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.WorkingHours(date, expertId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<string>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteService(Guid expertId, Guid serviceId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteService(expertId, serviceId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<string>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<WorkRoutineUpdate>> UpdateWorkRoutineData(Guid expertId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.UpdateWorkRoutineData(expertId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<WorkRoutineUpdate>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<ExpertWork>>> GetExpertCalendar(Guid expertId, int year, byte month)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetExpertCalendar(expertId, year, month);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<ExpertWork>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddItemToUserBasket(string userId, List<Guid> anlayses)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddItemToUserBasket(userId, anlayses);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<UpdateData>> UpdateData(Guid expertId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.UpdateData(expertId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UpdateData>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(UpdateData update, string ipAddress)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.Update(update, ipAddress);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdatePassword(Guid expertId, string oldPassword, string newPassword, string ipAddress)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.UpdatePassword(expertId, oldPassword, newPassword, ipAddress);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<ExpertPeriodGet>>> ExpertPeriods(Guid expertId, Guid? serviceId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.ExpertPeriods(expertId, serviceId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<ExpertPeriodGet>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdateExpertPeriod(ExpertPeriodGet periodGet)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.UpdateExpertPeriod(periodGet);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteExpertPeriod(Guid expertId, Guid expertPeriodId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteExpertPeriod(expertId, expertPeriodId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Entities.DTOs.AppointmentsDtos.Body.ExpertAppointmentDetailed>> AppointmentDetailed(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.AppointmentDetailed(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Entities.DTOs.AppointmentsDtos.Body.ExpertAppointmentDetailed>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Entities.DTOs.AppointmentsDtos.Body.ExpertAppointment>>> Appointments(Guid expertId, int page)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.Appointments(expertId, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Entities.DTOs.AppointmentsDtos.Body.ExpertAppointment>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}