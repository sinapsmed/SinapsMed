using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Analyses;
using Entities.Concrete.CuponCodes;
using Entities.Concrete.Services;
using Entities.Concrete.UserEntities;
using Entities.DTOs.CuponDtos;
using Entities.DTOs.CuponDtos.Body;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Cupons.CRUD
{
    public class EFCuponReadDAL : EFCuponCodeAdapter
    {
        private readonly IRepositoryBase<Cupon, Get, AppDbContext> _cupon;
        private readonly IRepositoryBase<CuponUsing, UsedCupon, AppDbContext> _usedCupon;
        private readonly IRepositoryBase<Service, CuponService, AppDbContext> _service;
        private readonly IRepositoryBase<Analysis, CuponService, AppDbContext> _analysis;
        private readonly IRepositoryBase<AppUser, CuponService, AppDbContext> _user;

        public EFCuponReadDAL(
            IRepositoryBase<Cupon, Get, AppDbContext> cupon,
            AppDbContext context,
            IStringLocalizer<EFCuponCodeAdapter> dalLocalizer,
            IRepositoryBase<CuponUsing, UsedCupon, AppDbContext> usedCupon,
            IRepositoryBase<Service, CuponService, AppDbContext> service,
            IRepositoryBase<Analysis, CuponService, AppDbContext> analysis,
            IRepositoryBase<AppUser, CuponService, AppDbContext> user) : base(context, dalLocalizer)
        {
            _cupon = cupon;
            _usedCupon = usedCupon;
            _service = service;
            _analysis = analysis;
            _user = user;
        }

        public override async Task<IDataResult<Update>> UpdateData(Guid cuponId)
        {
            var querry = await _context.Set<Cupon>()
                .Include(c => c.UsedCupons)
                .FirstOrDefaultAsync(c => c.Id == cuponId);

            if (querry is null)
                return new ErrorDataResult<Update>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            var data = CuponSelector.UpdateData(querry);

            return new SuccessDataResult<Update>(data, HttpStatusCode.OK);
        }
        public async override Task<IDataResult<BaseDto<Get>>> GetAll(int page, int limit)
        {
            var querry = _context.Set<Cupon>()
                .Include(c => c.UsedCupons).AsQueryable();

            var limitter = new DtoFilter<Cupon, Get>
            {
                Limit = limit,
                Page = page,
                Selector = CuponSelector.GetCupon()
            };

            return await _cupon.GetAllAsync(querry, limitter);
        }
        public async override Task<IDataResult<BaseDto<UsedCupon>>> GetUsedCupons(Guid cuponId, int page)
        {
            var querry = _context.Set<CuponUsing>()
                .OrderByDescending(c => c.UsedAt)
                .Include(c => c.User)
                .Where(c => c.CuponId == cuponId);

            if (querry is null)
                return new ErrorDataResult<BaseDto<UsedCupon>>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            var filter = new DtoFilter<CuponUsing, UsedCupon>
            {
                Limit = 10,
                Page = page <= 1 ? 2 : page,
                Selector = CuponSelector.UsedCupons()
            };

            return await _usedCupon.GetAllAsync(querry, filter);
        }
        public async override Task<IDataResult<Detail>> GetById(Guid id)
        {
            var querry = await _context.Set<Cupon>()
                .Include(c => c.SpesficCuponUsers)
                    .ThenInclude(c => c.User)
                .Include(c => c.SpesficServiceCupons)
                .Include(c => c.UsedCupons.OrderByDescending(x => x.UsedAt).Take(10))
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (querry is null)
                return new ErrorDataResult<Detail>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            Detail detail = CuponSelector.GetDetail(querry);

            if (querry.Type is CuponType.Appointment)
            {
                foreach (var spesficServiceCupon in querry.SpesficServiceCupons)
                {
                    var data = await _context.Set<Service>()
                        .Include(c => c.Languages)
                        .FirstOrDefaultAsync(c => c.Id == spesficServiceCupon.ServiceId);

                    if (data is null)
                    {
                        detail.Services.Add(new ServiceCuponDetail
                        {
                            Id = Guid.NewGuid(),
                            ImageUrl = "Something was worng...",
                            IsValidService = false,
                            Title = "Something was worng..."
                        });
                    }
                    else
                    {
                        detail.Services.Add(new ServiceCuponDetail
                        {
                            Id = spesficServiceCupon.ServiceId,
                            ImageUrl = data.ImageUrl,
                            IsValidService = false,
                            Title = data.Languages.FirstOrDefault(c => c.Code == _cultre).Title
                        });
                    }
                }
            }
            else if (querry.Type is CuponType.Analysis)
            {
                foreach (var spesficServiceCupon in querry.SpesficServiceCupons)
                {
                    var data = await _context.Set<Analysis>()
                        .FirstOrDefaultAsync(c => c.Id == spesficServiceCupon.ServiceId);

                    if (data is null)
                    {
                        detail.Services.Add(new AnalysisCuponDetail
                        {
                            Id = spesficServiceCupon.ServiceId,
                            Name = "Something was worng...",
                            IsValidAnalysis = false,
                            Code = "Something was worng..."
                        });
                    }
                    else
                    {
                        detail.Services.Add(new AnalysisCuponDetail
                        {
                            Id = spesficServiceCupon.ServiceId,
                            IsValidAnalysis = true,
                            Code = data.Code,
                            Name = data.Name,
                        });
                    }
                }
            }

            return new SuccessDataResult<Detail>(detail);
        }
        public override async Task<IDataResult<byte>> Discount(string code, string? userName, CuponType type)
        {
            var cupon = await _context.Set<Cupon>()
                .Include(c => c.UsedCupons)
                .FirstOrDefaultAsync(c => c.Code == code);

            if (cupon is null)
                return new ErrorDataResult<byte>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            if (cupon.UsedCupons.Count() >= cupon.UseLimit)
                return new ErrorDataResult<byte>(_dalLocalizer["useLimitReached"], HttpStatusCode.NotFound);

            if (cupon.ExpiredAt <= DateTime.UtcNow)
                return new ErrorDataResult<byte>(_dalLocalizer["expiredCupon"], HttpStatusCode.NotFound);

            if (cupon.Type != type)
                return new ErrorDataResult<byte>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, _dalLocalizer["wrongTypeCupon"]);

            var user = await _context.Users
                .FirstOrDefaultAsync(c => c.UserName == userName);

            if (user is null)
                return new ErrorDataResult<byte>(_dalLocalizer["unAuthorized"], HttpStatusCode.NotFound);

            if (cupon.UsedCupons.Where(c => c.UserId == user.Id).Count() >= cupon.UseLimitForPerUser)
                return new ErrorDataResult<byte>(_dalLocalizer["userLimitReached"], HttpStatusCode.NotFound);

            return new SuccessDataResult<byte>(data: cupon.Discount);
        }
        public override async Task<IDataResult<BaseDto<CuponService>>> GetServices(CuponServiceBody body)
        {
            if (body.Type is CuponType.Appointment)
            {
                var querry = _context.Set<Service>()
                    .Include(c => c.Languages)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(body.Search))
                    querry = querry.Where(c => c.Languages.Any(c => c.Title.ToLower().Contains(body.Search.ToLower())));

                var filter = new DtoFilter<Service, CuponService>
                {
                    Limit = 10,
                    Page = body.Page,
                    Selector = CuponSelector.GetServices(_cultre)
                };

                return await _service.GetAllAsync(querry, filter);
            }
            else if (body.Type is CuponType.Analysis)
            {
                var querry = _context.Set<Analysis>()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(body.Search))
                    querry = querry.Where(c => c.Name.ToLower().Contains(body.Search.ToLower()) || c.Code.ToLower().Contains(body.Search.ToLower()));

                var filter = new DtoFilter<Analysis, CuponService>
                {
                    Limit = 10,
                    Page = body.Page,
                    Selector = c => new CuponService
                    {
                        Id = c.Id.ToString(),
                        Label = $"{c.Name} ({c.Code})"
                    }
                };

                return await _analysis.GetAllAsync(querry, filter);
            }
            else
            {
                var serviceQuery = _context.Set<Service>()
                    .Include(c => c.Languages)
                    .AsQueryable();

                var analysisQuery = _context.Set<Analysis>()
                    .AsQueryable();


                if (!string.IsNullOrWhiteSpace(body.Search))
                {
                    serviceQuery = serviceQuery.Where(c => c.Languages.Any(c => c.Title.ToLower().Contains(body.Search.ToLower())));
                    analysisQuery = analysisQuery.Where(c => c.Name.ToLower().Contains(body.Search.ToLower()) || c.Code.ToLower().Contains(body.Search.ToLower()));
                }

                var serviceFilter = new DtoFilter<Service, CuponService>
                {
                    Limit = 5,
                    Page = body.Page,
                    Selector = CuponSelector.GetServices(_cultre)
                };

                var analysisFilter = new DtoFilter<Analysis, CuponService>
                {
                    Limit = 5,
                    Page = body.Page,
                    Selector = c => new CuponService
                    {
                        Id = c.Id.ToString(),
                        Label = $"{c.Name} ({c.Code})"
                    }
                };

                var servicesResult = await _service.GetAllAsync(serviceQuery, serviceFilter);
                var analysisResult = await _analysis.GetAllAsync(analysisQuery, analysisFilter);

                var combinedData = servicesResult.Data.Data
                    .Concat(analysisResult.Data.Data)
                    .ToList();

                var combinedResult = new BaseDto<CuponService>
                {
                    Data = combinedData,
                    CurrentPage = servicesResult.Data.CurrentPage,
                    PageCount = servicesResult.Data.PageCount + analysisResult.Data.PageCount
                };

                return new SuccessDataResult<BaseDto<CuponService>>(combinedResult, HttpStatusCode.OK);
            }
        }
        public override async Task<IDataResult<BaseDto<CuponService>>> GetUsers(CuponUserBody body)
        {
            var querry = _context.Set<AppUser>()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(body.Search))
            {
                querry = querry.Where(c => c.UnicalKey.ToLower().Contains(body.Search) || c.FullName.ToLower().Contains(body.Search));
            }

            var filter = new DtoFilter<AppUser, CuponService>
            {
                Limit = 10,
                Page = body.Page,
                Selector = c => new CuponService
                {
                    Id = c.Id.ToString(),
                    Label = $"{c.FullName} ({c.UnicalKey})"
                }
            };
            return await _user.GetAllAsync(querry, filter);
        }
    }
}