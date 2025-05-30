using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.ExpertDtos;
using Entities.DTOs.ExpertDtos.AdminDtos.Get;
using Entities.DTOs.ExpertDtos.BodyDtos;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Entities.Enums.Appointment;
using Create = Entities.DTOs.ExpertDtos.Create;

namespace DataAccess.Abstract
{
    public interface IExpertDAL
    {
        //AddCreate
        Task<IResult> AddItemToUserBasket( string userId, List<Guid> anlayses);
        Task<IResult> AddPause(CreateWorkPause pause);
        Task<IResult> AddHoliday(CreateWorkHoliday holiday);
        Task<IResult> AddServices(Guid expertId, IEnumerable<Guid> serviceIds);
        Task<IResult> AddPeriod(Guid expertId, double price, Guid periodId);
        Task<IResult> AddExpert(Create create);

        //Read 
        Task<IDataResult<BaseDto<ExpertAppointment>>> Appointments(string expertId, AppointmentStatus? appointmentStatus, int page);
        Task<IDataResult<ExpertAppointmentDetailed>> AppointmentDetailed(Guid id);
        Task<IDataResult<BaseDto<ExpertAppointment>>> Appointments(Guid expertId, int page);
        Task<IDataResult<List<ExpertPeriodGet>>> ExpertPeriods(Guid expertId, Guid? serviceId);
        Task<IDataResult<UpdateData>> UpdateData(Guid expertId);
        Task<IDataResult<WorkRoutineUpdate>> UpdateWorkRoutineData(Guid expertId);
        Task<IDataResult<List<ExpertWork>>> GetExpertCalendar(Guid expertId, int year, byte month);
        Task<IDataResult<BaseDto<ExpertList>>> GetAll(Guid? serviceId, string search, int page, int limit);
        Task<IDataResult<BaseDto<ExpertDetailedList>>> GetAllDetailed(Guid? serviceId, string search, int page, int limit);
        Task<IDataResult<BaseDto<GetBoosted>>> GetBoosted(int limit, int page);
        Task<IDataResult<ExpertServiceDto>> Services(Guid id, Guid? serviceId);
        Task<IDataResult<List<string>>> WorkingDays(Guid expertId);
        Task<IDataResult<List<string>>> WorkingHours(DateTime date, Guid expertId);

        //Update 
        Task<IResult> UpdatePassword(Guid expertId, string oldPassword, string newPassword, string ipAddress);
        Task<IResult> Update(UpdateData update, string ipAddress);
        Task<IResult> UpdateWorkRoutine(WorkRoutineUpdate routineCreate);
        Task<IResult> UpdateExpertPeriod(ExpertPeriodGet periodGet);

        //Delete
        Task<IResult> DeleteService(Guid expertId, Guid serviceId);
        Task<IResult> DeleteExpertPeriod(Guid expertId, Guid expertPeriodId);
    }
}