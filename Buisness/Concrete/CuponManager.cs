using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Cupon;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.CuponDtos;
using Entities.DTOs.CuponDtos.Body;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class CuponManager : Manager, ICuponService
    {
        private readonly IStringLocalizer<CommonLocalizer> _localizer;
        private readonly CuponServiceFactory _serviceFactory;
        public CuponManager(IStringLocalizer<CommonLocalizer> localizer, CuponServiceFactory serviceFactory)
        {
            _localizer = localizer;
            _serviceFactory = serviceFactory;
        }

        public async Task<IResult> Create(Create create)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Create);
                var result = await _dal.Create(create);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Delete(Guid id)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Delete);
                var result = await _dal.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<byte>> Discount(string code, string? userName, CuponType type)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Read);
                var result = await _dal.Discount(code, userName, type);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<byte>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Get>>> GetAll(int page, int limit)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Read);
                var result = await _dal.GetAll(page, limit);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Get>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Detail>> GetById(Guid id)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Read);
                var result = await _dal.GetById(id);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Detail>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<CuponService>>> GetServices(CuponServiceBody body)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Read);
                var result = await _dal.GetServices(body);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<CuponService>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<UsedCupon>>> GetUsedCupons(Guid cuponId, int page)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Read);
                var result = await _dal.GetUsedCupons(cuponId, page);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<UsedCupon>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<CuponService>>> GetUsers(CuponUserBody body)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Read);
                var result = await _dal.GetUsers(body);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<CuponService>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(Update update)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Update);
                var result = await _dal.Update(update);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Update>> UpdateData(Guid cuponId)
        {
            try
            {
                var _dal = _serviceFactory.GetService(ServiceFactoryType.Read);
                var result = await _dal.UpdateData(cuponId);
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Update>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}