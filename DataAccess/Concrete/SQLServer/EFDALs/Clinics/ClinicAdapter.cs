using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.ClinicDtos.DataDtos;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Clinics
{
    public class ClinicAdapter : BaseAdapter, IClinicDAL
    {
        protected readonly IStringLocalizer<ClinicAdapter> _dalLocalizer;
        public ClinicAdapter(AppDbContext context, IStringLocalizer<ClinicAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IResult> CheckAsUsed(Guid clinicId, Guid itemId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<ClinicDetail>> ClinicDetail(Guid clinicId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<OrderItemDetail>> ItemDetail(Guid clinicId, Guid? itemId, string? code)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<ClinicOrderItem>>> Orders(Guid? clinicId, int page)
        {
            throw new NotImplementedException();
        }
    }
}