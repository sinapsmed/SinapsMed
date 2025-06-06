using System.Net;
using Core.DataAccess;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD;
using DataAccess.Services.Abstract;
using Entities.Concrete.Appointments;
using Entities.Concrete.Experts;
using Entities.Concrete.Experts.WorkTimes;
using Entities.Concrete.Services;
using Entities.DTOs.BasketDtos.BodyDtos;
using Entities.DTOs.ExpertDtos;
using Entities.DTOs.ExpertDtos.BodyDtos;
using Entities.Enums.Appointment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD
{
    public class EFExpertCreateDAL : ExpertAdapter
    {
        private readonly IRepositoryBase<Expert, GetBoosted, AppDbContext> _repo;
        private readonly IConfiguration _config;
        private readonly IEmailService _email;
        private readonly IGoogleService _google;
        private readonly EFBasketCreateDAL _basket;
        private readonly IDAtaAccessService _service;

        public EFExpertCreateDAL(
            IRepositoryBase<Expert, GetBoosted, AppDbContext> repo,
            AppDbContext context,
            IStringLocalizer<ExpertAdapter> dalLocalizer, IEmailService email,
            IDAtaAccessService service,
            IConfiguration config, EFBasketCreateDAL basket, IGoogleService google) : base(context, dalLocalizer)
        {
            _repo = repo;
            _email = email;
            _service = service;
            _config = config;
            _basket = basket;
            _google = google;
        }

        public override async Task<IResult> AddExpert(Create create)
        {
            if (await _context.Experts.AnyAsync(c => c.Email.ToLower() == create.Email.ToLower()))
                return new ErrorResult(_dalLocalizer["emailAlreadyUsed", create.Email.ToLower()], HttpStatusCode.BadRequest, $"Email already used in diffrend account");

            string password = _service.GeneratePasswrod();

            string hexString = _config["Hashing:Default"];

            byte[] salt = hexString
                    .Split('-')
                    .Select(hex => Convert.ToByte(hex, 16))
                    .ToArray();

            string hashed = _service.HashPassword(password, salt);

            Expert expert = new Expert
            {
                Boosted = false,
                Email = create.Email,
                FullName = create.FullName,
                Resume = create.Resume,
                Specality = create.Specality,
                Services = new List<Service>(),
                PasswordHash = hashed,
                PhotoUrl = create.PhotoUrl,
                IsActive = false,
                Fee = create.Fee
            };

            foreach (var id in create.ServiceId)
            {
                var service = await _context.Services
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (service is null)
                    return new ErrorResult(_dalLocalizer["specalityNotFound"], HttpStatusCode.NotFound, $"We can not Find Expert Specality");

                expert.Services.Add(service);
            }

            var result = await _repo.AddAsync(expert, _context);

            if (result.Success)
            {
                var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "exppassword.html");
                string fileContent = File.ReadAllText(url);

                fileContent = fileContent
                    .Replace("{{username}}", expert.FullName)
                    .Replace("{{password}}", _dalLocalizer["mailPassword", password])
                    .Replace("{{thanks}}", _dalLocalizer["mailThanks"])
                    .Replace("{{activateUrl}}", _google.GoogleLogin(expert.Email).Data)
                    .Replace("{{text}}", _dalLocalizer["mailText", expert.FullName]);

                await _email.SendEmailAsync(expert.Email, _dalLocalizer["mail"], fileContent);
            }

            return result;
        }

        public override async Task<IResult> AddServices(Guid expertId, IEnumerable<Guid> serviceIds)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Services)
                .AsQueryable()
                .FirstOrDefaultAsync(c => c.Id == expertId);

            //null check 
            if (expert is null)
                return new ErrorResult(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, $"We can not find expert with id {expertId}");


            foreach (var id in serviceIds)
            {
                var service = await _context.Services
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (service is null)
                    return new ErrorResult(_dalLocalizer["serviceNotFound"], HttpStatusCode.NotFound, $"We can not find service with id {id}");

                //cehck if service already added
                if (expert.Services.Any(c => c.Id == id))
                    continue;

                expert.Services.Add(service);
            }

            await _context.SaveChangesAsync();
            return new SuccessResult(HttpStatusCode.OK);
        }

        public override async Task<IResult> AddPeriod(Guid expertId, double price, Guid periodId)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Services)
                    .ThenInclude(c => c.Periods)
                .Include(c => c.ServicePeriods)
                .AsQueryable()
                .FirstOrDefaultAsync(c => c.Id == expertId);

            //null check 
            if (expert is null)
                return new ErrorResult(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, $"We can not find expert with id {expertId}");

            if (!expert.Services.SelectMany(c => c.Periods).Any(x => x.Id == periodId))
                return new ErrorResult(_dalLocalizer["periodNotFound"], HttpStatusCode.NotFound, $"We can not find period with id {periodId}");

            if (expert.ServicePeriods.Any(c => c.Id == periodId))
                return new ErrorResult(_dalLocalizer["periodAlreadyAdded"], HttpStatusCode.BadRequest, $"Period already added");

            expert.ServicePeriods.Add(new ExpertServicePeriod
            {
                Price = price,
                ExpertId = expert.Id,
                ServicePeriodId = periodId
            });

            await _context.SaveChangesAsync();
            return new SuccessResult(HttpStatusCode.OK);
        }

        public override async Task<IResult> AddHoliday(CreateWorkHoliday holiday)
        {
            var any = await _context.Set<WorkHoliday>()
                .FirstOrDefaultAsync(c => c.Start <= holiday.Start && c.End >= holiday.End && c.IsGlobal);

            if (any is not null)
                return new ErrorResult(_dalLocalizer["holidayAlreadyAdded", any.HolidayTitle], HttpStatusCode.BadRequest, $"Holiday already added");

            var hol = new WorkHoliday
            {
                Start = holiday.Start,
                End = holiday.End,
                HolidayTitle = holiday.HolidayTitle,
                IsGlobal = holiday.IsGlobal,
            };

            if (!holiday.IsGlobal)
            {
                foreach (var id in holiday.Experts)
                {
                    var expert = await _context.Experts
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (expert is null)
                        return new ErrorResult(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, $"We can not find expert with id {id}");

                    hol.Experts.Add(expert);
                }
            }

            await _context.Set<WorkHoliday>().AddAsync(hol);
            await _context.SaveChangesAsync();

            return new SuccessResult("Uğurla Əlavə edildi!", HttpStatusCode.OK);
        }


        public override async Task<IResult> AddPause(CreateWorkPause pause)
        {
            var expert = await _context.Experts
                .Include(c => c.Pauses)
                .FirstOrDefaultAsync(c => c.Id == pause.ExpertId);

            if (expert is null)
                return new ErrorResult(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, $"We can not find expert with id {pause.ExpertId}");

            //cehck pause already added 
            if (expert.Pauses.Any(c => c.Start <= pause.Start && c.End >= pause.End))
                return new ErrorResult(_dalLocalizer["pauseAlreadyAdded"], HttpStatusCode.BadRequest, $"Pause already added");

            var workPause = pause.Map<WorkPause, CreateWorkPause>();

            expert.Pauses.Add(workPause);

            await _context.SaveChangesAsync();
            return new SuccessResult(HttpStatusCode.OK);
        }

        public override async Task<IResult> AddItemToUserBasket(string userId, List<Guid> anlayses)
        {
            foreach (var analysis in anlayses)
            {
                AddItem item = new()
                {
                    AnalysisId = analysis,
                    AppointmentId = null,
                    Count = 1,
                    UserId = userId
                };

                var response = await _basket.AddItem(item);

                if (!response.Success)
                    return response;
            }

            return new SuccessResult();
        }
    }
}