using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Forms.Diagnoses;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Entities.Enums.Appointment;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Appointments
{
    public class AppointmentAdapter : BaseAdapter, IAppointmentDAL
    {
        protected readonly IStringLocalizer<AppointmentAdapter> _dalLocalizer;
        public AppointmentAdapter(AppDbContext context, IStringLocalizer<AppointmentAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IResult> AddAnamnezFormDiagnosis(AnamnezCreate create)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> AddFile(string filePath, string title, Guid appointmentId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<byte[]>> AnamnezFormGet(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<AppointmentFormData>> AppointmentFormData(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Create(Create create)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<AppointmentDetail>> Detail(Guid id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IDataResult<BaseDto<AppointmentList>>> GetAll(Superiority periority, Guid? expertId, string userId, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<Diagnosis>>> GetDiagnoses(string search)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> NotifyExpert(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<AppointmentSchedule>>> Schedule(ReqFrom reqFrom, Guid? expertId, Guid? serviceId, AppointmentStatus? status, int year, int month)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UploadDiagnostics(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}