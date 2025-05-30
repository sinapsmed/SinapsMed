using System.Net;
using Core.DataAccess;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Services;
using Entities.DTOs.ServiceDtos.Create;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.SpecalitiyDtos.Create;
using Entities.DTOs.SpecalitiyDtos.Get;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Services.CRUD
{
    public class EFServiceCreateDAL : ServiceAdapter
    {
        private readonly IRepositoryBase<ServicePeriod, PeriodDto, AppDbContext> _periodServ;

        public EFServiceCreateDAL(
            AppDbContext context,
            IStringLocalizer<ServiceAdapter> dalLocalizer,
            IRepositoryBase<Service, GetSpecality, AppDbContext> servRepo,
            IRepositoryBase<Service, GetService, AppDbContext> servAdm,
            IRepositoryBase<ServicePeriod, PeriodGetDto, AppDbContext> periodGServ,
            IRepositoryBase<ServiceCategory, GetCat, AppDbContext> catRepo,
            IRepositoryBase<ServicePeriod, PeriodDto, AppDbContext> periodServ) : base(context, dalLocalizer, servRepo, servAdm, periodGServ, catRepo)
        {
            _periodServ = periodServ;
        }

        public override async Task<IResult> AddComplaint(CreateComplaint complaint)
        {
            var service = await _context.Services
                .Include(c => c.Complaints)
                .FirstOrDefaultAsync(c => c.Id == complaint.ServiceId);

            if (service is null)
                return new ErrorResult(_dalLocalizer["serviceIdNotFound"], HttpStatusCode.NotFound, "Service Id is crashed or invalid");

            var data = complaint.Complaints
                    .Select(c => new Complaint
                    {
                        ServiceId = service.Id,
                        Title = c
                    }).ToList();

            service.Complaints.AddRange(data);

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
        public override async Task<IResult> AddPeriod(PeriodDto period)
        {
            if (!await _context.Services.AnyAsync(c => c.Id == period.ServiceId))
                return new ErrorResult(_dalLocalizer["serviceIdNotFound"], HttpStatusCode.NotFound, "Service Id is crashed or invalid");

            var model = new ServicePeriod
            {
                ServiceId = period.ServiceId,
                Duration = period.Duration,
                Languages = period.Periods.Select(c => new ServicePeriodLang
                {
                    Code = c.Code,
                    PeriodText = c.Title,
                }).ToList()
            };

            return await _periodServ.AddAsync(model, _context);
        }
        public override async Task<IResult> AddService(CreateSpecailty createSpecailty)
        {
            if (!await _context.ServiceCategories.AnyAsync(c => c.Id == createSpecailty.CategoryId))
                return new ErrorResult(_dalLocalizer["catIdNotFound"], HttpStatusCode.NotFound, "Category Id is crashed or invalid");
            var model = new Service
            {
                ImageUrl = createSpecailty.LogoUrl,
                Languages = createSpecailty.Data.Select(c => new ServiceLang
                {
                    Code = c.Code,
                    Title = c.Title,
                    Description = c.Description,
                    Href = SeoHelper.ConverToSeo(c.Title, _cultre)
                }).ToList(),
                CategoryId = createSpecailty.CategoryId
            };

            return await _servRepo.AddAsync(model, _context);
        }
        public override async Task<IResult> CreateCategory(List<CreateCat> cats)
        {
            var model = new ServiceCategory
            {
                Languages = cats.Select(c => new ServiceCategoryLang
                {
                    Title = c.Title,
                    Code = c.Code,
                    Href = SeoHelper.ConverToSeo(c.Title, _cultre)

                }).ToList()
            };

            return await _catRepo.AddAsync(model, _context);
        }
    }
}