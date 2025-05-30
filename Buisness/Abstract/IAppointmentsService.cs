using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.Forms.Diagnoses;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Entities.Enums.Appointment;

namespace Buisness.Abstract
{
    public interface IAppointmentsService
    {
        //Read
        Task<IDataResult<AppointmentFormData>> AppointmentFormData(Guid id);
        Task<IDataResult<List<AppointmentSchedule>>> Schedule(ReqFrom reqFrom, Guid? expertId, Guid? serviceId, AppointmentStatus? status, int year, int month);
        Task<IDataResult<byte[]>> AnamnezForm(Guid id);
        Task<IDataResult<AppointmentDetail>> Detail(Guid id);
        Task<IDataResult<BaseDto<AppointmentList>>> GetAll(Superiority periority, Guid? expertId, string? userId, int page);
        Task<IDataResult<BaseDto<Diagnosis>>> GetDiagnoses(string search);

        //Custom 
        Task<IResult> NotifyExpert(Guid id);
        Task<IResult> UploadDiagnostics(Microsoft.AspNetCore.Http.IFormFile file);

        //Create, Add
        Task<IResult> AddAnamnezFormDiagnosis(AnamnezCreate create);
        Task<IResult> Create(Create create);
        Task<IResult> AddFile(Microsoft.AspNetCore.Http.IFormFile file, string title, Guid appointmentId);
    }
}