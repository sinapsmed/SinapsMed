using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.OrderDtos.BodyDtos;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Orders.CRUD
{
    public class EFOrderReadDAL : OrderAdapter
    {
        private readonly IRepositoryBase<Order, GetAll, AppDbContext> _order;
        public EFOrderReadDAL(
            AppDbContext context,
            IStringLocalizer<OrderAdapter> dalLocalizer,
            IRepositoryBase<Order, GetAll, AppDbContext> order) : base(context, dalLocalizer)
        {
            _order = order;
        }

        public override async Task<IDataResult<DetailedOrder>> Detailed(Guid id)
        {
            var order = await _context.Set<Order>()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (order is null)
                return new ErrorDataResult<DetailedOrder>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            var result = new DetailedOrder
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Number = order.Number,
                Items = order.Items.Select(c => new OrderItemDto
                {
                    Amount = c.Amount,
                    Count = c.Count,
                    Id = c.Id,
                    IsUsed = c.IsUsed,
                    Type = c.Type,
                    UnikalKey = c.UnikalKey,
                    Title = OrderSelector.ItemTitle(c, _cultre),
                    ClinicDetail = OrderSelector.GetClinicDetail(c)
                }).ToList()
            };

            return new SuccessDataResult<DetailedOrder>(result, _dalLocalizer["success"], HttpStatusCode.OK);
        }

        public override async Task<IDataResult<BaseDto<GetAll>>> GetAll(string unikalKey, string userName, Superiority superiority, int page)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(c => c.UnicalKey == unikalKey);

            if (user is null)
                return new ErrorDataResult<BaseDto<GetAll>>(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            if (superiority is not Superiority.Admin && user.UserName != userName)
            {
                return new ErrorDataResult<BaseDto<GetAll>>(_dalLocalizer["unAuth"], HttpStatusCode.Unauthorized);
            }

            var querry = _context.Set<Order>()
                .Include(c => c.Items)
                .Where(c => c.UserId == user.Id);

            var filter = new DtoFilter<Order, GetAll>
            {
                Limit = 10,
                Page = page,
                Selector = OrderSelector.GetAll(_cultre)
            };

            return await _order.GetAllAsync(querry, filter);

        }
    }
}