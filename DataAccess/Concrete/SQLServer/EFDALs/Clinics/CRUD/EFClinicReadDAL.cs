using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Clinics;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.ClinicDtos.DataDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Clinics.CRUD
{
    public class EFClinicReadDAL : ClinicAdapter
    {

        private readonly IRepositoryBase<OrderItem, ClinicOrderItem, AppDbContext> _repo;
        public EFClinicReadDAL(
            AppDbContext context,
            IStringLocalizer<ClinicAdapter> dalLocalizer,
            IRepositoryBase<OrderItem, ClinicOrderItem, AppDbContext> repo) : base(context, dalLocalizer)
        {
            _repo = repo;
        }

        public override async Task<IDataResult<ClinicDetail>> ClinicDetail(Guid clinicId)
        {
            var clinic = await _context.Set<Clinic>()
                .Include(c => c.Email)
                .FirstOrDefaultAsync(c => c.Id == clinicId);

            if (clinic is null)
                return new ErrorDataResult<ClinicDetail>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            var data = clinic.MapReverse<ClinicDetail, Clinic>();

            data.Email = clinic.Email.Email;

            return new SuccessDataResult<ClinicDetail>(data, HttpStatusCode.OK);

        }

        public override async Task<IDataResult<OrderItemDetail>> ItemDetail(Guid clinicId, Guid? itemId, string? code)
        {
            var clinic = await _context.Set<Clinic>()
                .Include(c => c.Orders)
                    .ThenInclude(c => c.Analysis)
                        .ThenInclude(c => c.Category)
                .Include(c => c.Orders)
                    .ThenInclude(c => c.Order)
                        .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == clinicId);

            if (clinic is null)
                return new ErrorDataResult<OrderItemDetail>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            List<OrderItem> items = new List<OrderItem>();
            if (itemId is null && code != null)
            {
                var codes = code.Split(" ");
                foreach (var item in codes)
                {
                    items.AddRange(clinic.Orders.Where(c => c.UnikalKey.ToLower() == item.ToLower()).ToList());
                }
            }
            else
            {
                items = clinic.Orders.Where(c => c.Id == itemId).ToList();
            }

            if (items.Count is 0)
                return new ErrorDataResult<OrderItemDetail>(_dalLocalizer["itemNotFound"], HttpStatusCode.NotFound);

            var data = ClinicSelector.ItemDetail(items);

            return new SuccessDataResult<OrderItemDetail>(data);
        }

        public override async Task<IDataResult<BaseDto<ClinicOrderItem>>> Orders(Guid? clinicId, int page)
        {
            var querry = _context.Set<OrderItem>()
                .Include(c => c.Order)
                    .ThenInclude(c => c.User)
                .OrderBy(c => !c.IsUsed)
                .AsQueryable();

            if (clinicId.HasValue)
                querry = querry.Where(c => c.ClinicId == clinicId);

            var dto = new DtoFilter<OrderItem, ClinicOrderItem>
            {
                Limit = 10,
                Page = page,
                Selector = ClinicSelector.OrderItems()
            };

            return await _repo.GetAllAsync(querry, dto);
        }
    }
}