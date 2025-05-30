using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AccountantDtos;
using Entities.DTOs.AddountantDtos.Body;
using Entities.DTOs.AddountantDtos.ReturnData;
using Entities.Enums.Accountant;
using Entities.Enums.Payments;

namespace DataAccess.Abstract
{
    public interface IAccountantDAL
    {
        Task<IDataResult<ClinicUperData<ClinicDto>>> ClinicSalesRecord(string? clinicKey, DateTime? start, DateTime? end, int page);
        Task<IDataResult<AppointmentsUpperData<AppointmentDtoData>>> AppointmentsSalesRecord(Guid? expertId, DateTime? start, DateTime? end, int page);
        Task<IDataResult<SalesRecord>> SalesRecord(SalesInterval interval);
        Task<IDataResult<SalesRecordForExpert>> SalesRecordForExpert(Guid expertId, SalesInterval interval);
        Task<IDataResult<SalesRecordForAnalyses>> SalesRecordForAnalyses(SalesInterval interval);
        Task<IDataResult<PaymentUperData<PaymentDto>>> Payments(string? orderNumber, string? cupon, DateTime? startDate, DateTime? endDate, PaymentStatus? status, int page, int limit);
        Task<IDataResult<DetailedPaymentDto>> PaymentDetail(int Id);
        Task<IResult> AddAsync(Create create);
        Task<IResult> UpdateAsync(Update update);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<Read>> GetByIdAsync(Guid id);
        Task<IDataResult<BaseDto<Read>>> GetAllAsync(int page, int limit);
    }
}