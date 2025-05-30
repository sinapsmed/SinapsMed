using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Appointments;
using Entities.Concrete.Forms;
using Entities.Concrete.Forms.Diagnoses;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.FormDtos;
using Entities.DTOs.Helpers;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.Users;
using Entities.Enums;
using Entities.Enums.Appointment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Appointments.CRUD
{
    public class EFAppointmentReadDAL : AppointmentAdapter
    {
        private readonly IRepositoryBase<Appointment, AppointmentList, AppDbContext> _repo;
        private readonly IRepositoryBase<Diagnosis, Diagnosis, AppDbContext> _diagnosisRepo;
        public EFAppointmentReadDAL(
            AppDbContext context,
            IStringLocalizer<AppointmentAdapter> dalLocalizer,
            IRepositoryBase<Appointment, AppointmentList, AppDbContext> repo,
            IRepositoryBase<Diagnosis, Diagnosis, AppDbContext> diagnosisRepo)
             : base(context, dalLocalizer)
        {
            _repo = repo;
            _diagnosisRepo = diagnosisRepo;
        }

        public override async Task<IDataResult<AppointmentFormData>> AppointmentFormData(Guid id)
        {
            var form = await _context.Set<Appointment>()
                .Include(c => c.ServicePeriod)
                    .ThenInclude(c => c.Service)
                        .ThenInclude(c => c.Languages)
                .Include(c => c.ServicePeriod)
                    .ThenInclude(c => c.Service)
                        .ThenInclude(c => c.Complaints)
                .Include(c => c.User)
                .Include(c => c.Expert)
                .Include(c => c.AdditionalUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (form is null)
                return new ErrorDataResult<AppointmentFormData>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            UserDetail ReturnCorrectUser(AppointmentType type)
            {
                if (type is AppointmentType.MySelf)
                    return new UserDetail
                    {
                        FullName = form.User.FullName,
                        UnicalKey = form.User.UnicalKey,
                        DateOfBrith = form.User.DateOfBrith is null ? "Undefined" : form.User.DateOfBrith.Value.ToString("dd.MM.yyyy"),
                        Gender = form.User.Gender
                    };
                else
                    return new UserDetail
                    {
                        FullName = form.AdditionalUser?.FullName ?? "Undefined",
                        UnicalKey = form.User.UnicalKey ?? "Undefined",
                        DateOfBrith = form.AdditionalUser.DateOfBrith.ToString("dd.MM.yyyy"),
                        Gender = form.AdditionalUser.Gender
                    };
            }

            DateTime CorrectUserBrith(AppointmentType type)
            {
                if (type is AppointmentType.MySelf)
                    return form.User.DateOfBrith is null ? DateTime.UtcNow : form.User.DateOfBrith.Value;
                else
                    return form.AdditionalUser.DateOfBrith;
            }

            var data = new AppointmentFormData
            {
                Complaints = form.ServicePeriod.Service.Complaints.Select(c => c.Title).ToList(),
                Date = form.Date.ToString("dd.MM.yyyy HH:mm"),
                ExpertFullName = form.Expert.FullName,
                Number = form.Number,
                ServiceName = form.ServicePeriod.Service.Languages
                    .FirstOrDefault(c => c.Code == _cultre).Title ?? "Undefined",
                User = ReturnCorrectUser(form.AppointmentType),
                AgeRange = AppointmentService.GetAgeRange(CorrectUserBrith(form.AppointmentType))
            };

            return new SuccessDataResult<AppointmentFormData>(data);

        }

        public override async Task<IDataResult<List<AppointmentSchedule>>> Schedule(ReqFrom reqFrom, Guid? oponentId, Guid? serviceId, AppointmentStatus? status, int year, int month)
        {
            DateTime start = new DateTime(year: year, month: month, 1);
            int lastDay = DateTime.DaysInMonth(year, month);
            DateTime end = new DateTime(year: year, month: month, lastDay);

            var appointments = _context.Set<Appointment>()
                .Where(c => c.Date >= start && c.Date <= end)
                .Include(c => c.ServicePeriod)
                    .ThenInclude(c => c.Service)
                        .ThenInclude(c => c.Languages)
                .Include(c => c.User)
                .Include(c => c.Expert)
                .AsQueryable();

            if (serviceId.HasValue)
                appointments = appointments.Where(c => c.ServicePeriod.ServiceId == serviceId);

            if (status.HasValue)
                appointments = appointments.Where(c => c.Status == status);

            if (reqFrom.Superiority is Superiority.Expert)
            {
                appointments = appointments.Where(c => c.ExpertId == reqFrom.RequesterId);

                if (oponentId.HasValue)
                    appointments = appointments.Where(c => c.UserId == oponentId.ToString());
            }

            if (reqFrom.Superiority is Superiority.User)
            {
                appointments = appointments.Where(c => c.UserId == reqFrom.UserId);

                if (oponentId.HasValue)
                    appointments = appointments.Where(c => c.ExpertId == oponentId);
            }

            var data = await appointments
                .OrderByDescending(c => c.Date)
                .Select(AppointmentSelector.Schedule(_dalLocalizer["apoMeetJoin"], reqFrom.Superiority))
                .ToListAsync();

            return new SuccessDataResult<List<AppointmentSchedule>>(data);
        }

        public override async Task<IDataResult<byte[]>> AnamnezFormGet(Guid id)
        {
            var form = await _context.Forms
            .Include(c => c.AnamnezFormDiagnoses)
                .ThenInclude(c => c.Diagnosis)
            .Include(c => c.Appointment)
                .ThenInclude(c => c.ServicePeriod)
                    .ThenInclude(c => c.Service)
                        .ThenInclude(c => c.Complaints)
            .Include(c => c.Appointment)
                .ThenInclude(c => c.ServicePeriod)
                    .ThenInclude(c => c.Service)
                        .ThenInclude(c => c.Languages)
            .Include(c => c.Appointment)
                .ThenInclude(c => c.Expert)
            .Include(c => c.Appointment)
                .ThenInclude(c => c.User)
            .Include(c => c.Appointment)
                .ThenInclude(c => c.AdditionalUser)
            .FirstOrDefaultAsync(c => c.Id == id);



            if (form is null)
                return new ErrorDataResult<byte[]>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            var data = form.MapReverse<AnamnezFormDetailed, AnamnezForm>();

            data.Diagnoses = form.AnamnezFormDiagnoses.Select(c => new DiagnosisDto
            {
                ICD10_Code = c.Diagnosis.ICD10_Code,
                Type = c.Type,
                WHO_Full_Desc = c.Diagnosis.WHO_Full_Desc
            }).ToList();


            if (form.Appointment.AppointmentType is not AppointmentType.MySelf && form.Appointment.AdditionalUser is not null)
            {
                form.Appointment.User.FullName = form.Appointment.AdditionalUser.FullName;
                form.Appointment.User.Gender = form.Appointment.AdditionalUser.Gender;
                form.Appointment.User.DateOfBrith = form.Appointment.AdditionalUser.DateOfBrith;
            }

            data.Expert = new ExpertDetail
            {
                Email = form.Appointment.Expert.Email,
                FullName = form.Appointment.Expert.FullName,
                PhotoUrl = form.Appointment.Expert.PhotoUrl
            };

            data.User = new DetailedUser
            {
                FullName = form.Appointment.User.FullName,
                UnikalId = form.Appointment.User.UnicalKey,
                DateOfBrith = form.Appointment.User.DateOfBrith,

            };

            data.Service = new PeriodGetDto
            {
                Id = form.Appointment.ServicePeriod.Id,
                Title = form.Appointment.ServicePeriod.Service.Languages
                    .FirstOrDefault(c => c.Code == _cultre).Title ?? "Undefined",
                Duration = form.Appointment.ServicePeriod.Duration
            };

            data.Status = form.Appointment.Status;

            data.Date = form.Appointment.Date;

            data.Number = form.Appointment.Number;

            data.Id = form.Appointment.Id;

            string pdfPath = AppointmentService.GeneratePdf(data);

            var pdf = File.ReadAllBytes(pdfPath);

            File.Delete(pdfPath);

            return new SuccessDataResult<byte[]>(pdf);
        }

        public override async Task<IDataResult<BaseDto<Diagnosis>>> GetDiagnoses(string search)
        {
            var querry = _context.Set<Diagnosis>().AsQueryable();

            if (search.Length >= 3)
            {
                querry = querry.Where(c => c.ICD10_Code.Contains(search) || c.WHO_Full_Desc.Contains(search));
            }

            DtoFilter<Diagnosis, Diagnosis> filter = new()
            {
                Limit = 100,
                Page = 1,
                Selector = c => c
            };

            return await _diagnosisRepo.GetAllAsync(querry, filter);
        }

        public override async Task<IDataResult<BaseDto<AppointmentList>>> GetAll(Superiority periority, Guid? expertId, string userId, int page)
        {
            var query = _context
                .Set<Appointment>()
                .Include(c => c.ServicePeriod)
                .ThenInclude(c => c.Service)
                .ThenInclude(c => c.Languages)
                .Include(c => c.User)
                .Include(c => c.Expert)
                .AsQueryable();

            if (periority is Superiority.User && !string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(c => c.UserId == userId);
                if (expertId.HasValue)
                {
                    query = query.Where(c => c.ExpertId == expertId);
                }
            }
            else if (periority is Superiority.Expert)
            {
                query = query.Where(c => c.ExpertId == expertId);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    query = query.Where(c => c.UserId == userId);
                }
            }
            else
            {
                if (expertId.HasValue)
                {
                    query = query.Where(c => c.ExpertId == expertId);
                }

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    query = query.Where(c => c.UserId == userId);
                }
            }

            DtoFilter<Appointment, AppointmentList> filter = new()
            {
                Limit = 5,
                Page = page,
                Selector = AppointmentSelector.ListAppointment(_cultre)
            };

            return await _repo.GetAllAsync(query, filter);
        }

        public async override Task<IDataResult<AppointmentDetail>> Detail(Guid id)
        {
            var query = await _context
                .Set<Appointment>()
                .Include(c => c.ServicePeriod)
                .ThenInclude(c => c.Service)
                .ThenInclude(c => c.Languages)
                .Include(c => c.User)
                .Include(c => c.Form)
                .Include(c => c.Expert)
                .Include(c => c.Attachments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (query is null)
                return new ErrorDataResult<AppointmentDetail>(_dalLocalizer["notFound"]);

            var data = query.Detail(_cultre);

            return new SuccessDataResult<AppointmentDetail>(data);
        }
    }
}