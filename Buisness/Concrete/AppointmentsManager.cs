using Buisness.Abstract;
using Buisness.Extentions;
using Buisness.Infrastructure.Factories.Appointments;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.Concrete.Forms.Diagnoses;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Entities.Enums.Appointment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class AppointmentsManager : IAppointmentsService
    {
        private readonly IStringLocalizer<Validator> _loclalizer;
        private readonly IFileService _fileService;
        private readonly AppointmentServiceFactory factory;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _axs;
        private readonly IWebHostEnvironment _host;

        public AppointmentsManager(IStringLocalizer<Validator> loclalizer, AppointmentServiceFactory factory, IWebHostEnvironment host, IFileService fileService, Microsoft.AspNetCore.Http.IHttpContextAccessor axs)
        {
            _loclalizer = loclalizer;
            this.factory = factory;
            _host = host;
            _fileService = fileService;
            _axs = axs;
        }

        public async Task<IResult> AddAnamnezFormDiagnosis(AnamnezCreate create)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Create);
                return await dal.AddAnamnezFormDiagnosis(create);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<byte[]>(_loclalizer["ex"], ex);
            }
        }

        public async Task<IResult> AddFile(Microsoft.AspNetCore.Http.IFormFile file, string title, Guid appointmentId)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Create);

                string url = string.Empty;

                var requestContext = _axs?.HttpContext?.Request;

                string scheme = requestContext?.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? requestContext?.Scheme;

                string folder = Path.Combine(_host.WebRootPath, "assets", "Appointments", appointmentId.ToString());

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var baseUrl = $"{scheme}://{requestContext?.Host}/assets/Appointments/" + appointmentId.ToString() + "/";

                url = baseUrl + await file.CreateFileAsync(_host, "assets", "Appointments", appointmentId.ToString());

                return await dal.AddFile(url, title, appointmentId);
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], ex);
            }
        }

        public async Task<IDataResult<byte[]>> AnamnezForm(Guid id)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Read);
                return await dal.AnamnezFormGet(id);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<byte[]>(_loclalizer["ex"], ex);
            }
        }

        public async Task<IDataResult<AppointmentFormData>> AppointmentFormData(Guid id)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Read);
                return await dal.AppointmentFormData(id);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<AppointmentFormData>(_loclalizer["ex"], ex);
            }
        }

        public async Task<IResult> Create(Create create)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Create);
                return await dal.Create(create);
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], ex);
            }
        }

        public async Task<IDataResult<AppointmentDetail>> Detail(Guid id)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Read);
                return await dal.Detail(id);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<AppointmentDetail>(_loclalizer["ex"], ex);
            }
        }

        public async Task<IDataResult<BaseDto<AppointmentList>>> GetAll(Superiority periority, Guid? expertId, string? userId, int page)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Read);
                return await dal.GetAll(periority, expertId, userId, page);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<AppointmentList>>(_loclalizer["ex"], ex);
            }
        }

        public async Task<IDataResult<BaseDto<Diagnosis>>> GetDiagnoses(string search)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Read);
                return await dal.GetDiagnoses(search);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Diagnosis>>(_loclalizer["ex"], ex);
            }
        }

        public async Task<IResult> NotifyExpert(Guid id)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Custom);
                return await dal.NotifyExpert(id);
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], ex);
            }
        }

        public async Task<IDataResult<List<AppointmentSchedule>>> Schedule(ReqFrom reqFrom, Guid? expertId, Guid? serviceId, AppointmentStatus? status, int year, int month)
        {
            try
            {
                var dal = factory.GetService(ServiceFactoryType.Read);
                return await dal.Schedule(reqFrom, expertId, serviceId, status, year, month);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<AppointmentSchedule>>(_loclalizer["ex"], ex);
            }
        }

        public async Task<IResult> UploadDiagnostics(Microsoft.AspNetCore.Http.IFormFile file)
        {
            try
            {
                string folder = Path.Combine(_host.WebRootPath, "assets", "Excels");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.Combine(folder, file.FileName);

                using (FileStream fileStream = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }

                var dal = factory.GetService(ServiceFactoryType.Custom);
                var response = await dal.UploadDiagnostics(fileName);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], ex);
            }
        }
    }
}