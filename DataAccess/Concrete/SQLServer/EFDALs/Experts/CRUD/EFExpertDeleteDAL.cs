using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Experts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD
{
    public class EFExpertDeleteDAL : ExpertAdapter
    {
        public EFExpertDeleteDAL(AppDbContext context, IStringLocalizer<ExpertAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }


        public override async Task<IResult> DeleteExpertPeriod(Guid expertId, Guid expertPeriodId)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.ServicePeriods)
                .FirstOrDefaultAsync(c => c.Id == expertId);

            if (expert is null)
                return new ErrorResult(_dalLocalizer["notFound"]);

            var servicePeriod = expert.ServicePeriods.FirstOrDefault(c => c.Id == expertPeriodId);

            if (servicePeriod is null)
                return new ErrorResult(_dalLocalizer["periodNotFound"]);

            expert.ServicePeriods.Remove(servicePeriod);
            await _context.SaveChangesAsync();

            return new SuccessResult();
        }

        public override async Task<IResult> DeleteService(Guid expertId, Guid serviceId)
        {
            var expert = await _context.Set<Expert>()
                .Include(c => c.Services)
                .Include(c => c.ServicePeriods)
                    .ThenInclude(c => c.ServicePeriod)
                .FirstOrDefaultAsync(c => c.Id == expertId);

            if (expert is null)
                return new ErrorResult(_dalLocalizer["notFound"]);

            var service = expert.Services.FirstOrDefault(c => c.Id == serviceId);

            if (service is null)
                return new ErrorResult(_dalLocalizer["serviceNotFound"]);

            if (expert.ServicePeriods.Any(c => c.ServicePeriod.ServiceId == service.Id))
                return new ErrorResult(_dalLocalizer["expertHasApointment"]);

            expert.Services.Remove(service);

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
    }
}