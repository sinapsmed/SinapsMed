using System.Net;
using AutoMapper;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Appointments;
using Entities.Concrete.Experts;
using Entities.Concrete.Experts.WorkTimes;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.ExpertDtos;
using Entities.DTOs.ExpertDtos.AdminDtos.Get;
using Entities.DTOs.ExpertDtos.BodyDtos;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Entities.Enums;
using Entities.Enums.Appointment;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Misc;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD
{
    public class EFExpertReadDAL : ExpertAdapter
    {
        private readonly IRepositoryBase<Expert, ExpertList, AppDbContext> _expertList;
        private readonly IRepositoryBase<Expert, ExpertDetailedList, AppDbContext> _expertDetailedList;
        private readonly IRepositoryBase<Expert, GetBoosted, AppDbContext> _repo;
        private readonly IRepositoryBase<Appointment, ExpertAppointment, AppDbContext> _apoRepo;
        public EFExpertReadDAL(
            IRepositoryBase<Expert, ExpertList, AppDbContext> expertList,
            IRepositoryBase<Expert, GetBoosted, AppDbContext> repo,
            AppDbContext context,
            IStringLocalizer<ExpertAdapter> dalLocalizer,
            IRepositoryBase<Expert, ExpertDetailedList, AppDbContext> expertDetailedList,
            IRepositoryBase<Appointment, ExpertAppointment, AppDbContext> apoRepo) : base(context, dalLocalizer)
        {
            _expertList = expertList;
            _repo = repo;
            _expertDetailedList = expertDetailedList;
            _apoRepo = apoRepo;
        }

        public override async Task<IDataResult<BaseDto<ExpertAppointment>>> Appointments(Guid expertId, int page)
        {
            var appointments = _context.Set<Appointment>()
                .Include(c => c.AdditionalUser)
                .Include(c => c.Expert)
                .Include(c => c.User)
                .Include(c => c.Attachments)
                .Where(c => c.ExpertId == expertId);

            var filter = new DtoFilter<Appointment, ExpertAppointment>
            {
                Limit = 10,
                Page = page,
                Selector = ExpertSelector.ExpertAppointments()
            };

            return await _apoRepo.GetAllAsync(appointments, filter);

        }
        public override async Task<IDataResult<List<ExpertPeriodGet>>> ExpertPeriods(Guid expertId, Guid? serviceId)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Services)
                    .ThenInclude(c => c.Periods)
                        .ThenInclude(c => c.Languages)
                .Include(c => c.ServicePeriods)
                    .ThenInclude(c => c.ServicePeriod)
                        .ThenInclude(c => c.Languages)
                .FirstOrDefaultAsync(e => e.Id == expertId);

            if (expert is null)
                return new ErrorDataResult<List<ExpertPeriodGet>>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            if (serviceId.HasValue)
            {
                expert.Services.Where(c => c.Id == serviceId).ToList();
                expert.ServicePeriods.Where(c => c.ServicePeriod.Service.Id == serviceId).ToList();
            }


            var data = new List<ExpertPeriodGet>();

            foreach (var item in expert.ServicePeriods)
            {
                data.Add(new ExpertPeriodGet
                {
                    Duration = item.ServicePeriod.Duration,
                    Id = item.ServicePeriod.Id,
                    Price = item.Price,
                    PeriodId = item.Id,
                    Title = item.ServicePeriod.Languages.FirstOrDefault(c => c.Code == _culture)?.PeriodText,
                    ExpertId = expertId,
                });
            }

            foreach (var item in expert.Services)
            {
                foreach (var period in item.Periods)
                {
                    if (data.Any(c => c.Id == period.Id))
                        continue;

                    data.Add(new ExpertPeriodGet
                    {
                        Duration = period.Duration,
                        Id = period.Id,
                        Title = period.Languages.FirstOrDefault(c => c.Code == _culture)?.PeriodText,
                        Price = null,
                        ExpertId = expertId,
                    });
                }
            }

            return new SuccessDataResult<List<ExpertPeriodGet>>(data: data);

        }
        public override async Task<IDataResult<WorkRoutineUpdate>> UpdateWorkRoutineData(Guid expertId)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Routine)
                    .ThenInclude(c => c.DayOfWeeks)
                        .ThenInclude(c => c.IntervalHours)
                .FirstOrDefaultAsync(e => e.Id == expertId);

            if (expert is null)
                return new ErrorDataResult<WorkRoutineUpdate>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            if (expert.Routine is null)
                return new SuccessDataResult<WorkRoutineUpdate>(new WorkRoutineUpdate()
                {
                    ExpertId = expertId,
                }, HttpStatusCode.OK);

            var data = new WorkRoutineUpdate()
            {
                ExpertId = expert.Id,
                Gap = expert.Routine.Gap,
                Interval = expert.Routine.Interval,
                WeekDayWorkTimes = new List<WeekDayWorkTime>()
            };

            foreach (var weekDay in expert.Routine.DayOfWeeks)
            {
                var workTime = new WeekDayWorkTime
                {
                    WeekDay = weekDay.WeekDay,
                    HoursInterval = new List<WeekDayHoursIntervalDto>()
                };

                foreach (var hoursInterval in weekDay.IntervalHours)
                {
                    workTime.HoursInterval.Add(new WeekDayHoursIntervalDto
                    {
                        Start = hoursInterval.Start,
                        End = hoursInterval.End
                    });
                }

                data.WeekDayWorkTimes.Add(workTime);
            }

            return new SuccessDataResult<WorkRoutineUpdate>(data);

        }
        public override async Task<IDataResult<List<ExpertWork>>> GetExpertCalendar(Guid expertId, int year, byte month)
        {
            DateTime start = new DateTime(year: year, month: month, 1);
            int lastDay = DateTime.DaysInMonth(year, month);
            DateTime end = new DateTime(year: year, month: month, lastDay);

            var expert = await _context.Set<Expert>()
                .Include(c => c.Holidays.Where(c => c.Start >= start && c.End <= end))
                .Include(c => c.Routine)
                    .ThenInclude(c => c.DayOfWeeks)
                        .ThenInclude(c => c.IntervalHours)
                .Include(c => c.Pauses)
                .FirstOrDefaultAsync(e => e.Id == expertId);

            expert.Holidays.AddRange(await _context.Set<WorkHoliday>().Where(c => c.IsGlobal).ToListAsync());

            if (expert is null)
                return new ErrorDataResult<List<ExpertWork>>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            if (expert.Routine is null)
                return new SuccessDataResult<List<ExpertWork>>(new List<ExpertWork>());

            var expertWorks = new List<ExpertWork>();

            var existingIntervals = new List<(DateTime Start, DateTime End)>();

            List<(DateTime Start, DateTime End)> SubtractOverlaps(DateTime start, DateTime end)
            {
                var result = new List<(DateTime, DateTime)> { (start, end) };

                foreach (var interval in existingIntervals)
                {
                    var updated = new List<(DateTime, DateTime)>();

                    foreach (var r in result)
                    {
                        if (interval.End <= r.Item1 || interval.Start >= r.Item2)
                        {
                            updated.Add(r);
                            continue;
                        }

                        if (interval.Start > r.Item1)
                            updated.Add((r.Item1, interval.Start));

                        if (interval.End < r.Item2)
                            updated.Add((interval.End, r.Item2));
                    }

                    result = updated;
                }

                return result;
            }


            foreach (var holiday in expert.Holidays)
            {
                expertWorks.Add(new ExpertWork
                {
                    Start = holiday.Start,
                    End = holiday.End,
                    Title = holiday.HolidayTitle,
                    Type = EventType.Holiday
                });

                existingIntervals.Add((holiday.Start, holiday.End));
            }

            foreach (var pause in expert.Pauses)
            {
                var pauseSegments = SubtractOverlaps(pause.Start, pause.End);

                foreach (var segment in pauseSegments)
                {
                    expertWorks.Add(new ExpertWork
                    {
                        Start = segment.Start,
                        End = segment.End,
                        Title = pause.Reason,
                        Type = EventType.Pause
                    });

                    existingIntervals.Add((segment.Start, segment.End));
                }
            }

            for (int i = 0; i < lastDay; i++)
            {
                DateTime thisDate = start.AddDays(i);
                var dayOfWeek = expert.Routine.DayOfWeeks.FirstOrDefault(c => (int)c.WeekDay == (int)thisDate.DayOfWeek);
                if (dayOfWeek is null)
                    continue;

                foreach (var interval in dayOfWeek.IntervalHours)
                {
                    DateTime dayStartTime = thisDate.Add(interval.Start);
                    DateTime dayEndTime = thisDate.Add(interval.End);

                    while (dayStartTime < dayEndTime)
                    {
                        System.Console.WriteLine("Loop Found");
                        DateTime intervalEnd = dayStartTime.Add(expert.Routine.Interval);
                        if (intervalEnd > dayEndTime)
                            break;

                        var segments = SubtractOverlaps(dayStartTime, intervalEnd);
                        foreach (var segment in segments)
                        {
                            expertWorks.Add(new ExpertWork
                            {
                                Start = segment.Start,
                                End = segment.End,
                                Title = "İş vaxtı",
                                Type = EventType.WorkDay
                            });

                            existingIntervals.Add(segment);
                        }

                        dayStartTime = intervalEnd.Add(expert.Routine.Gap);
                    }
                }

            }

            return new SuccessDataResult<List<ExpertWork>>(expertWorks);


        }
        public override async Task<IDataResult<BaseDto<GetBoosted>>> GetBoosted(int limit, int page)
        {
            var entity = _context.Experts.Where(c => c.Boosted && !c.IsSuspend);

            var filter = new DtoFilter<Expert, GetBoosted>
            {
                Limit = limit,
                Page = page,
                Selector = ExpertSelector.BoostedExpert(_culture)
            };
            return await _repo.GetAllAsync(entity, filter);
        }
        public override async Task<IDataResult<List<string>>> WorkingDays(Guid expertId)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Holidays)
                .Include(c => c.Pauses)
                .Include(e => e.Routine)
                    .ThenInclude(r => r.DayOfWeeks)
                .FirstOrDefaultAsync(e => e.Id == expertId);

            if (expert is null)
                return new ErrorDataResult<List<string>>(
                    _dalLocalizer["expertNotFound"],
                    HttpStatusCode.NotFound,
                    "Expert id is invalid or crashed");

            var globalHolidays = await _context.Set<WorkHoliday>().Where(c => c.IsGlobal).ToListAsync();
            var allHolidays = globalHolidays.Concat(expert.Holidays).ToList();

            DateTime startDate = DateTime.UtcNow.Date.AddDays(1);
            var routine = expert.Routine;

            if (routine?.DayOfWeeks == null || !routine.DayOfWeeks.Any())
                return new SuccessDataResult<List<string>>(new List<string>());

            var result = new List<string>();
            int counter = 0;

            while (counter < 7)
            {
                DateTime currentDate = startDate.AddDays(counter);
                var weekDayEnum = (WeekDay)currentDate.DayOfWeek;

                if (routine.DayOfWeeks.Any(d => d.WeekDay == weekDayEnum))
                {
                    var holiday = allHolidays.FirstOrDefault(h =>
                        h.Start.Date <= currentDate && h.End.Date >= currentDate);
                    if (holiday != null)
                    {
                        result.Add($"{currentDate:yyyy-MM-dd}");
                    }
                    else
                    {
                        var pause = expert.Pauses.FirstOrDefault(p =>
                            p.Start.Date <= currentDate && p.End.Date >= currentDate);
                        if (pause != null)
                        {
                            result.Add($"{currentDate:yyyy-MM-dd}");
                        }
                        else
                        {
                            result.Add($"{currentDate:yyyy-MM-dd}");
                        }
                    }
                }
                counter++;
            }

            return new SuccessDataResult<List<string>>(result);

        }
        public override async Task<IDataResult<BaseDto<ExpertDetailedList>>> GetAllDetailed(Guid? serviceId, string search, int page, int limit)
        {

            IQueryable<Expert> query = _context
                .Set<Expert>()
                .Include(c => c.Services)
                    .ThenInclude(c => c.Languages)
                .Include(c => c.Services)
                    .ThenInclude(c => c.Category);

            if (serviceId.HasValue)
            {
                query = query.Where(expert => expert.Services.Any(s => s.Id == serviceId.Value));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(expert => expert.FullName.ToLower().Contains(search.ToLower()));

            }

            var filter = new DtoFilter<Expert, ExpertDetailedList>
            {
                Page = page,
                Limit = limit,
                Selector = ExpertSelector.ExpertDetailedList(_culture)
            };

            return await _expertDetailedList.GetAllAsync(query, filter);
        }
        public override async Task<IDataResult<ExpertServiceDto>> Services(Guid id, Guid? serviceId)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Services)
                    .ThenInclude(c => c.Category)
                        .ThenInclude(c => c.Languages)
                .Include(c => c.Services)
                    .ThenInclude(c => c.Languages)
                .Include(c => c.Services)
                    .ThenInclude(c => c.Periods)
                        .ThenInclude(c => c.ExpertPeriods)
                .Include(c => c.Services)
                    .ThenInclude(c => c.Periods)
                        .ThenInclude(c => c.Languages)
                .AsQueryable()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (expert is null)
                return new ErrorDataResult<ExpertServiceDto>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            IEnumerable<ExpertServicePeriod> expertServices;

            if (serviceId is not null)
            {
                expertServices = expert.ServicePeriods.Where(x => x.Id == serviceId);
            }

            var data = expert.Services
                .Select(c => new ExpertServices
                {
                    ServiceId = c.Id,
                    ServiceName = c.Languages.FirstOrDefault(c => c.Code == _culture).Title,
                    SubServices = c.Periods
                     .SelectMany(x => x.ExpertPeriods.Where(c => c.ExpertId == expert.Id))
                        .Select(z => new ExpertSubServices
                        {
                            Price = z.Price,
                            Duration = z.ServicePeriod.Duration,
                            Id = z.ServicePeriod.Id,
                            LogoUrl = z.ServicePeriod.Service.ImageUrl,
                            Title = z.ServicePeriod.Languages.FirstOrDefault(c => c.Code == _culture).PeriodText
                        }).ToList()
                }).ToList();


            return new SuccessDataResult<ExpertServiceDto>(new ExpertServiceDto { Expert = $"{expert.Specality} {expert.FullName}", ExpertServices = data }, HttpStatusCode.OK);
        }
        public override async Task<IDataResult<BaseDto<ExpertList>>> GetAll(Guid? serviceId, string search, int page, int limit)
        {
            IQueryable<Expert> query = _context
                .Set<Expert>()
                .Where(x => x.IsActive && !x.IsSuspend)
                .Include(c => c.Services)
                    .ThenInclude(c => c.Languages);

            if (serviceId.HasValue)
            {
                query = query.Where(expert => expert.Services.Any(s => s.Id == serviceId.Value));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(expert => expert.FullName.ToLower().Contains(search.ToLower()));

            }

            var filter = new DtoFilter<Expert, ExpertList>
            {
                Page = page,
                Limit = limit,
                Selector = ExpertSelector.ExpertList(_culture)
            };

            return await _expertList.GetAllAsync(query, filter);
        }
        public override async Task<IDataResult<List<string>>> WorkingHours(DateTime date, Guid expertId)
        {
            var expert = await _context.Set<Expert>()
                .Include(e => e.Routine)
                    .ThenInclude(r => r.DayOfWeeks)
                        .ThenInclude(c => c.IntervalHours)
                .Include(c => c.Holidays)
                .Include(c => c.Pauses)
                .FirstOrDefaultAsync(e => e.Id == expertId);

            if (expert is null)
                return new ErrorDataResult<List<string>>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            var workRoutine = expert.Routine;
            if (workRoutine == null || workRoutine.DayOfWeeks == null || !workRoutine.DayOfWeeks.Any())
                return new SuccessDataResult<List<string>>(new List<string>());

            var globalHolidays = await _context.WorkHolidays.Where(c => c.IsGlobal).ToListAsync();

            expert.Holidays.AddRange(globalHolidays);

            if (expert.Holidays.Any(c => c.Start <= date && c.End >= date))
                return new SuccessDataResult<List<string>>(new List<string>());

            var intervals = expert.Routine.DayOfWeeks.FirstOrDefault(c => (int)c.WeekDay == (int)date.DayOfWeek);

            if (intervals is null)
            {
                return new SuccessDataResult<List<string>>(new List<string>());
            }
            var hours = intervals.IntervalHours;

            var data = new List<string>();
            foreach (var interval in hours)
            {
                DateTime workStart = date.Date.Add(interval.Start);
                DateTime workEnd = date.Date.Add(interval.End);

                var pauses = expert.Pauses
                    .OrderBy(p => p.Start)
                    .ToList();

                while (workStart < workEnd)
                {
                    var pause = pauses.FirstOrDefault(p => p.Start <= workStart && p.End >= workStart);

                    if (pause != null)
                    {
                        workStart = workStart.Add(expert.Routine.Interval + expert.Routine.Gap);
                        continue;
                    }

                    data.Add(workStart.ToString("HH:mm"));
                    workStart = workStart.Add(expert.Routine.Interval + expert.Routine.Gap);
                }
            }


            return new SuccessDataResult<List<string>>(data);
        }
        public override async Task<IDataResult<UpdateData>> UpdateData(Guid expertId)
        {
            var expert = await _context.Set<Expert>()
                .FirstOrDefaultAsync(e => e.Id == expertId);

            if (expert is null)
                return new ErrorDataResult<UpdateData>(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound, "Expert id is invalid or crashed");

            var data = new UpdateData
            {
                ExpertId = expert.Id,
                FullName = expert.FullName,
                IsActive = expert.IsActive,
                IsSuspend = expert.IsSuspend,
                IsBoosted = expert.Boosted,
                PhotoUrl = expert.PhotoUrl,
                Resume = expert.Resume,
                Fee = expert.Fee,
                Specality = expert.Specality,
                Email = expert.Email
            };
            return new SuccessDataResult<UpdateData>(data);
        }
    }
}