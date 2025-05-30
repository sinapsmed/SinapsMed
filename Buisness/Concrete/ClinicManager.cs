using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Clinics;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class ClinicManager : ICLinicService
    {
        private readonly IStringLocalizer<CommonLocalizer> _localizer;
        private readonly ClinicServiceFactory _factory;

        public ClinicManager(ClinicServiceFactory factory, IStringLocalizer<CommonLocalizer> localizer)
        {
            _factory = factory;
            _localizer = localizer;
        }

        public async Task<IResult> CheckAsUsed(Guid clinicId, Guid itemId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var result = await _dal.CheckAsUsed(clinicId, itemId);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<ClinicDetail>> ClinicDetail(Guid clinicId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var result = await _dal.ClinicDetail(clinicId);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ClinicDetail>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<OrderItemDetail>> ItemDetail(Guid clinicId, Guid? itemId, string? code)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var result = await _dal.ItemDetail(clinicId, itemId, code);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<OrderItemDetail>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<ClinicOrderItem>>> Orders(Guid? clinicId, int page)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var result = await _dal.Orders(clinicId, page);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<ClinicOrderItem>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}