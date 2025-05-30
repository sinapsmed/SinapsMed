using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Accountants;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.AccountantDtos;
using Entities.DTOs.AddountantDtos.Body;
using Entities.DTOs.AddountantDtos.ReturnData;
using Entities.Enums;
using Entities.Enums.Accountant;
using Entities.Enums.Payments;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class AccountantManager : IAccountantService
    {
        private readonly AccountantsServiceFactory _factory;
        private readonly IStringLocalizer<Validator> _loclalizer;
        public AccountantManager(AccountantsServiceFactory factory, IStringLocalizer<Validator> loclalizer)
        {
            _factory = factory;
            _loclalizer = loclalizer;
        }

        public async Task<IResult> AddAsync(Create create)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await dal.AddAsync(create);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<AppointmentsUpperData<AppointmentDtoData>>> AppointmentsSalesRecord(Guid? expertId, DateTime? start, DateTime? end, int page)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.AppointmentsSalesRecord(expertId, start, end, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<AppointmentsUpperData<AppointmentDtoData>>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<ClinicUperData<ClinicDto>>> ClinicSalesRecord(string? clinicKey, DateTime? start, DateTime? end, int page)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.ClinicSalesRecord(clinicKey, start, end, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ClinicUperData<ClinicDto>>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await dal.DeleteAsync(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Read>>> GetAllAsync(int page, int limit)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.GetAllAsync(page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Read>>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Read>> GetByIdAsync(Guid id)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.GetByIdAsync(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Read>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<DetailedPaymentDto>> PaymentDetail(int Id)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.PaymentDetail(Id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<DetailedPaymentDto>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<PaymentUperData<PaymentDto>>> Payments(string? orderNumber, string? cupon, DateTime? startDate, DateTime? endDate, PaymentStatus? status, int page, int limit)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.Payments(orderNumber, cupon, startDate, endDate, status, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<PaymentUperData<PaymentDto>>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<SalesRecord>> SalesRecord(SalesInterval interval)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.SalesRecord(interval);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<SalesRecord>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<SalesRecordForAnalyses>> SalesRecordForAnalyses(SalesInterval interval)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.SalesRecordForAnalyses(interval);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<SalesRecordForAnalyses>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<SalesRecordForExpert>> SalesRecordForExpert(Guid expertId, SalesInterval interval)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.SalesRecordForExpert(expertId, interval);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<SalesRecordForExpert>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdateAsync(Update update)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await dal.UpdateAsync(update);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}