using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.AccountantDtos;
using Entities.DTOs.AddountantDtos.Body;
using Entities.DTOs.AddountantDtos.ReturnData;
using Entities.Enums.Accountant;
using Entities.Enums.Payments;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Accountatnts
{
    public class AccountantAdapter : BaseAdapter, IAccountantDAL
    {
        protected IStringLocalizer<AccountantAdapter> _dalLocalizer;
        public AccountantAdapter(AppDbContext context, IStringLocalizer<AccountantAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IResult> AddAsync(Create create)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<AppointmentsUpperData<AppointmentDtoData>>> AppointmentsSalesRecord(Guid? expertId, DateTime? start, DateTime? end, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<ClinicUperData<ClinicDto>>> ClinicSalesRecord(string? clinicKey, DateTime? start, DateTime? end, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<Read>>> GetAllAsync(int page, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<Read>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<DetailedPaymentDto>> PaymentDetail(int Id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<PaymentUperData<PaymentDto>>> Payments(string? orderNumber, string? cupon, DateTime? startDate, DateTime? endDate, PaymentStatus? status, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<SalesRecord>> SalesRecord(SalesInterval interval)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<SalesRecordForAnalyses>> SalesRecordForAnalyses(SalesInterval interval)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<SalesRecordForExpert>> SalesRecordForExpert(Guid expertId, SalesInterval interval)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UpdateAsync(Update update)
        {
            throw new NotImplementedException();
        }
    }
}