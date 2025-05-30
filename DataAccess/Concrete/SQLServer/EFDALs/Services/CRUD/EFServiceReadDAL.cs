using System.Net;
using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.Concrete.Services;
using Entities.DTOs.Helpers;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.ServiceDtos.Update;
using Entities.DTOs.SpecalitiyDtos.Get;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Services.CRUD
{
    public class EFServiceReadDAL : ServiceAdapter
    {
        private readonly IRepositoryBase<ServiceCategory, GetHeader, AppDbContext> _catHeader;
        public EFServiceReadDAL(
            AppDbContext context,
            IStringLocalizer<ServiceAdapter> dalLocalizer,
            IRepositoryBase<Service, GetSpecality, AppDbContext> servRepo,
            IRepositoryBase<Service, GetService, AppDbContext> servAdm,
            IRepositoryBase<ServicePeriod, PeriodGetDto, AppDbContext> periodGServ,
            IRepositoryBase<ServiceCategory, GetCat, AppDbContext> catRepo,
            IRepositoryBase<ServiceCategory, GetHeader, AppDbContext> catHeader) : base(context, dalLocalizer, servRepo, servAdm, periodGServ, catRepo)
        {
            _catHeader = catHeader;
        }

        public override async Task<IDataResult<List<GetCat>>> GetCategories()
        {
            var queries = _context.Set<ServiceCategory>().Include(c => c.Languages);

            var selector = ServiceSelector.Categories(_cultre);

            return await _catRepo.GetAllAsync(queries, selector);
        }
        public override async Task<IDataResult<IEnumerable<GetComplaints>>> GetComplaints(Guid serviceId)
        {
            var service = await _context.Services
                .Include(c => c.Complaints)
                .FirstOrDefaultAsync(c => c.Id == serviceId);

            if (service is null)
                return new ErrorDataResult<IEnumerable<GetComplaints>>(_dalLocalizer["serviceIdNotFound"], HttpStatusCode.NotFound, "Service Id is crashed or invalid");

            var data = service.Complaints
                    .Select(c => new GetComplaints
                    {
                        Id = c.Id,
                        Title = c.Title
                    }).ToList();

            return new SuccessDataResult<IEnumerable<GetComplaints>>(data, HttpStatusCode.OK);

        }
        public override async Task<IDataResult<List<GetHeader>>> GetHeaders()
        {
            var queries = _context.Set<ServiceCategory>()
                .Include(c => c.Languages)
                .Include(c => c.Services)
                .ThenInclude(c => c.Languages);

            var selector = ServiceSelector.Header(_cultre);

            return await _catHeader.GetAllAsync(queries, selector);
        }
        public override async Task<IDataResult<List<GetService>>> GetServices(Guid? id, Guid? expertId)
        {
            var queries = _context
                .Set<Service>()
                .Include(c => c.Languages)
                .AsQueryable();

            if (id.HasValue)
                queries = queries.Where(c => c.CategoryId == id);

            if (expertId.HasValue)
                queries = queries.Where(c => c.Experts.Any(x => x.Id == expertId));

            var selector = ServiceSelector.Services(_cultre);

            return await _servAdm.GetAllAsync(queries, selector);
        }
        public override async Task<IDataResult<CategoryUpdateGet>> UpdateCategoryData(Guid id)
        {
            var selector = ServiceSelector.UpdateCategoryGet();

            var data = await _context
               .ServiceCategories
               .Include(c => c.Languages)
               .Select(selector)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (data is null)
                throw new DataNullException(id.ToString(), _cultre);

            return new SuccessDataResult<CategoryUpdateGet>(data: data, HttpStatusCode.OK);
        }
        public override async Task<IDataResult<ServiceUpdateGet>> UpdateServiceData(Guid id)
        {
            var selector = ServiceSelector.UpdateGet();

            var data = await _context
               .Services
               .Include(c => c.Languages)
               .Select(selector)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (data is null)
                throw new DataNullException(id.ToString(), _cultre);

            return new SuccessDataResult<ServiceUpdateGet>(data: data, HttpStatusCode.OK);
        }
        public override async Task<IDataResult<ServicePeriodUpdateGet>> UpdateServicePeriodData(Guid id)
        {
            var selector = ServiceSelector.UpdatePeriodGet();

            var data = await _context
               .ServicePeriods
               .Include(c => c.Languages)
               .Select(selector)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (data is null)
                throw new DataNullException(id.ToString(), _cultre);

            return new SuccessDataResult<ServicePeriodUpdateGet>(data: data, HttpStatusCode.OK);
        }
        public override async Task<IDataResult<List<PeriodGetDto>>> Periods(Guid specId, ReqFrom from)
        {
            var entity = _context.Set<ServicePeriod>()
                .Where(c => c.ServiceId == specId);

            if (from.Superiority is Superiority.Expert)
            {
                entity.Where(c => c.Service.Experts.Any(x => x.Id == from.RequesterId));
            }

            var selector = ServiceSelector.Period(_cultre);

            return await _periodGServ.GetAllAsync(entity, selector);
        }
        public override async Task<IDataResult<List<GetSpecality>>> AllServices(int page, Guid? expertId)
        {
            var entity = _context.Set<Service>()
                .Include(c => c.Experts)
                .AsQueryable();

            if (expertId.HasValue)
            {
                entity = entity.Where(c => c.Experts.Any(x => x.Id == expertId));
            }

            var selector = ServiceSelector.AllService(_cultre);

            return await _servRepo.GetAllAsync(entity, selector: selector);
        }
        public override async Task<IDataResult<Detail>> ServiceDetail(Guid id)
        {
            var entity = await _context.Services
                .Include(c => c.Category)
                .ThenInclude(c => c.Languages)
                .Include(c => c.Languages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity is null)
                return new ErrorDataResult<Detail>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, _dalLocalizer["notFound"]);

            var data = ServiceSelector.DetailFuc(entity, _cultre);

            return new SuccessDataResult<Detail>(data, HttpStatusCode.OK);
        }
    }
}