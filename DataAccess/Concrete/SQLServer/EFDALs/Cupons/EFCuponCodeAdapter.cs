using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.CuponDtos;
using Entities.DTOs.CuponDtos.Body;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Cupons
{
    public class EFCuponCodeAdapter : BaseAdapter, ICuponDAL
    {
        protected IStringLocalizer<EFCuponCodeAdapter> _dalLocalizer;
        public EFCuponCodeAdapter(AppDbContext context, IStringLocalizer<EFCuponCodeAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IResult> Create(Create create)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<byte>> Discount(string code, string? userName, CuponType type)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<Get>>> GetAll(int page, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<Detail>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<CuponService>>> GetServices(CuponServiceBody body)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<CuponService>>> GetUsers(CuponUserBody body)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<UsedCupon>>> GetUsedCupons(Guid cuponId, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Update(Update update)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<Update>> UpdateData(Guid cuponId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UseCupon(string userId, string code, double amount)
        {
            throw new NotImplementedException();
        }
    }
}