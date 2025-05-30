using System.Net;
using Core.DataAccess;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using DataAccess.Services.Abstract;
using Entities.Concrete.Experts;
using Entities.Concrete.Experts.WorkTimes;
using Entities.DTOs.ExpertDtos;
using Entities.DTOs.ExpertDtos.BodyDtos;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using StackExchange.Redis;

namespace DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD
{
    public class EFExpertUpdateDAL : ExpertAdapter
    {
        private readonly IConfiguration _config;
        private readonly IGeoLocationService _geoLocationService;
        private readonly IEmailService _email;
        private readonly IDAtaAccessService _service;
        public EFExpertUpdateDAL(
            IRepositoryBase<Expert, GetBoosted, AppDbContext> repo,
            AppDbContext _context,
            IStringLocalizer<ExpertAdapter> dalLocalizer,
            IConfiguration config,
            IDAtaAccessService service,
            IEmailService email,
            IGeoLocationService geoLocationService) : base(_context, dalLocalizer)
        {
            _config = config;
            _service = service;
            _email = email;
            _geoLocationService = geoLocationService;
        }

        public override async Task<IResult> UpdateExpertPeriod(ExpertPeriodGet periodGet)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Services)
                    .ThenInclude(c => c.Periods)
                .Include(c => c.ServicePeriods)
                .FirstOrDefaultAsync(e => e.Id == periodGet.ExpertId);

            if (expert is null)
                return new ErrorResult(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            var servicePeriod = expert.ServicePeriods.FirstOrDefault(sp => sp.Id == periodGet.PeriodId);
            
            if (servicePeriod == null)
                return new ErrorResult(_dalLocalizer["servicePeriodNotFound"], HttpStatusCode.NotFound, "Service period not found");

            if (periodGet.Price.HasValue)
                servicePeriod.Price = periodGet.Price.Value;

            await _context.SaveChangesAsync();

            return new SuccessResult(HttpStatusCode.OK);
        }
        public override async Task<IResult> UpdateWorkRoutine(WorkRoutineUpdate routineCreate)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Routine)
                    .ThenInclude(c => c.DayOfWeeks)
                .AsQueryable()
                .FirstOrDefaultAsync(c => c.Id == routineCreate.ExpertId);

            if (expert is null)
                return new ErrorResult(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, $"We can not find expert with id {routineCreate.ExpertId}");

            if (routineCreate.Interval < TimeSpan.FromMinutes(10))
                return new ErrorResult(_dalLocalizer["gapTimeError"], HttpStatusCode.BadRequest, "Interval time must be greater than 10 minute");

            var routine = new WorkRoutine
            {
                ExpertId = expert.Id,
                Gap = routineCreate.Gap,
                Interval = routineCreate.Interval,
                DayOfWeeks = routineCreate.WeekDayWorkTimes
                    .Select(c => new WorkRoutineWeekDay
                    {
                        WeekDay = c.WeekDay,
                        IntervalHours = c.HoursInterval
                            .Select(d => new WrokIntervalHoursData
                            {
                                Start = d.Start,
                                End = d.End
                            }).ToList()
                    }).ToList()
            };

            expert.Routine = routine;

            await _context.SaveChangesAsync();
            return new SuccessResult(HttpStatusCode.OK);
        }
        public override async Task<IResult> UpdatePassword(Guid expertId, string oldPassword, string newPassword, string ipAddress)
        {
            var expert = await _context.Set<Expert>()
                .FirstOrDefaultAsync(e => e.Id == expertId);

            if (expert is null)
                return new ErrorDataResult<UpdateData>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            string defaultHash = _config["Hashing:Default"];

            byte[] defaultSalt = defaultHash
                    .Split('-')
                    .Select(hex => Convert.ToByte(hex, 16))
                    .ToArray();

            if (_service.HashPassword(oldPassword, defaultSalt) != expert.PasswordHash)
                return new ErrorResult(_dalLocalizer["wrongPassword"]);

            var passwordCheck = _service.CheckPasswordRequirements(newPassword);

            if (!passwordCheck.Success)
                return passwordCheck;

            expert.PasswordHash = _service.HashPassword(newPassword, defaultSalt);

            var geoLocation = await _geoLocationService.GetGeoLocation(ipAddress);

            await _context.SaveChangesAsync();

            var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "passwordchange.html");
            string fileContent = File.ReadAllText(url);

            fileContent = fileContent
                .Replace("{{name}}", $"Hörmətli Mütəxəssis {expert.FullName}")
                .Replace("{{country}}", geoLocation.Data.Country)
                .Replace("{{city}}", geoLocation.Data.City);

            await _email.SendEmailAsync(
                expert.Email,
                "Şifrə Dəyişdirildi",
                fileContent
            );

            return new SuccessResult();
        }
        public override async Task<IResult> Update(UpdateData update, string ipAddress)
        {
            var expert = await _context.Set<Expert>()
                .FirstOrDefaultAsync(e => e.Id == update.ExpertId);

            if (expert is null)
                return new ErrorDataResult<UpdateData>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            string firstName = expert.FullName;

            expert.FullName = update.FullName;
            expert.Boosted = update.IsBoosted;
            expert.PhotoUrl = update.PhotoUrl;
            expert.IsSuspend = update.IsSuspend;
            expert.IsActive = update.IsActive;
            expert.Resume = update.Resume;
            expert.Fee = update.Fee;

            await _context.SaveChangesAsync();
            return new SuccessResult();
        }

    }
}