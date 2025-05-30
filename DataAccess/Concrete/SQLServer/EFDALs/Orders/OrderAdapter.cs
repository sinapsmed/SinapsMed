using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.OrderDtos.BodyDtos;
using Entities.DTOs.PaymentDtos;
using Entities.Enums;
using Entities.Enums.Payments;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Orders
{
    public class OrderAdapter : BaseAdapter, IOrderDAL
    {

        protected IStringLocalizer<OrderAdapter> _dalLocalizer;
        public OrderAdapter(AppDbContext context, IStringLocalizer<OrderAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IDataResult<RedirectionDto>> CheckOutOrder(string user, string? cupon)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<DetailedOrder>> Detailed(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<GetAll>>> GetAll(string unikalKey, string userName, Superiority superiority, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Payment(int paymentId, PaymentStatus status, string? unikalKey = null)
        {
            throw new NotImplementedException();
        }
    }
}