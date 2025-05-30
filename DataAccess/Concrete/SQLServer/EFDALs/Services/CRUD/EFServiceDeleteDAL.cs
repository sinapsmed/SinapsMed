using System.Net;
using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.Concrete.Services;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.SpecalitiyDtos.Get;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Services.CRUD
{
    public class EFServiceDeleteDAL : ServiceAdapter
    {
        public EFServiceDeleteDAL(
            AppDbContext context,
            IStringLocalizer<ServiceAdapter> dalLocalizer,
            IRepositoryBase<Service, GetSpecality, AppDbContext> servRepo,
            IRepositoryBase<Service, GetService, AppDbContext> servAdm,
            IRepositoryBase<ServicePeriod, PeriodGetDto, AppDbContext> periodGServ,
            IRepositoryBase<ServiceCategory, GetCat, AppDbContext> catRepo) : base(context, dalLocalizer, servRepo, servAdm, periodGServ, catRepo)
        {
        }

        public override async Task<IDataResult<string>> Delete(Guid id)
        {
            var querry = _context.Set<Service>()
                    .Include(c => c.Languages)
                    .Include(c => c.Periods)
                    .FirstOrDefault(c => c.Id == id);

            if (querry is null)
                throw new DataNullException(id.ToString(), _cultre);

            var imageUrl = querry.ImageUrl;

            var response = await _servAdm.Remove(querry, _context);
            if (response.Success)
                return new SuccessDataResult<string>(data: imageUrl, HttpStatusCode.OK);
            else
                return new ErrorDataResult<string>(response.Message, response.StatusCode);
        }
        public override async Task<IResult> DeleteCategory(Guid id)
        {
            var data = _context.Set<ServiceCategory>()
                .Include(c => c.Services)
                .Include(c => c.Languages)
                .FirstOrDefault(x => x.Id == id);

            if (data is null)
                throw new DataNullException(id.ToString(), _cultre);

            if (data.Services.Count() > 0)
                return new ErrorResult(_dalLocalizer["cantDelete"], HttpStatusCode.BadRequest, "Service Category can't delete when its have any service");

            _context.Set<ServiceCategoryLang>().RemoveRange(data.Languages);

            _context.Set<ServiceCategory>().Remove(data);

            await _context.SaveChangesAsync();

            return new SuccessResult(HttpStatusCode.OK);
        }
        public override async Task<IResult> DeleteComplaint(Guid id)
        {
            var data = await _context.Set<Complaint>().FirstOrDefaultAsync(c => c.Id == id);

            if (data is null)
                throw new DataNullException(id.ToString(), _cultre);

            _context.Set<Complaint>().Remove(data);

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
        public override async Task<IResult> DeletePeriod(Guid id)
        {
            var querry = _context.Set<ServicePeriod>()
                    .Include(c => c.Languages)
                    .FirstOrDefault(c => c.Id == id);

            if (querry is null)
                throw new DataNullException(id.ToString(), _cultre);

            return await _periodGServ.Remove(querry, _context);
        }
    }
}