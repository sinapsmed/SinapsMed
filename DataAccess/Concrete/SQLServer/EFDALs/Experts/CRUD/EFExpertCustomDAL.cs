using DataAccess.Concrete.SQLServer.DataBase;
using Microsoft.Extensions.Localization;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Entities.Concrete.Experts;
using Entities.Concrete.Experts.WorkTimes;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD
{
    public class EFExpertCustomDAL : ExpertAdapter
    {
        private readonly EFExpertReadDAL _expertRead;
        public EFExpertCustomDAL(AppDbContext context, IStringLocalizer<ExpertAdapter> dalLocalizer, EFExpertReadDAL expertRead) : base(context, dalLocalizer)
        {
            _expertRead = expertRead;
        }

        public async Task<bool> IsWorkingTime(Guid expertId, DateTime dateTime)
        {
            var hours = await _expertRead.WorkingHours(dateTime, expertId);

            if (hours == null)
                return false;

            if (hours.Data.Any(c => c == dateTime.ToString("HH:mm")))
                return true;

            return false;
        }
        public async Task<IResult> CancelBronExpertTime(Guid expertId, DateTime start)
        {
            var pause = await _context.Set<WorkPause>().FirstOrDefaultAsync(p =>
                p.ExpertId == expertId &&
                p.Start == start);

            if (pause == null)
                return new ErrorResult("No matching manual Bron found");

            _context.Set<WorkPause>().Remove(pause);
            await _context.SaveChangesAsync();

            return new SuccessResult("Bron successfully cancelled");
        }
        public async Task<IResult> BronExpertTime(Guid expertId, DateTime start, TimeSpan duration)
        {
            var expert = await _context.Set<Expert>()
                .Include(e => e.Pauses)
                .FirstOrDefaultAsync(e => e.Id == expertId);

            if (expert == null)
                return new ErrorResult("Expert not found");

            var end = start.Add(duration);

            var pause = new WorkPause
            {
                Start = start,
                End = end,
                ExpertId = expertId,
                Reason = "Bu Tarix hal hazirda başqa istifadəçinin səbətində bron edilib. 15 dəqiqə ərzində ödəniş edilməsə açılacaqdır."
            };

            expert.Pauses.Add(pause);

            await _context.SaveChangesAsync();

            return new SuccessResult("Time blocked successfully");
        }

    }
}