using System.Net;
using Buisness.Abstract;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using Entities.Concrete.Helpers;
using Entities.DTOs.Helpers;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class CustomManager : Manager, ICustomService
    {
        private readonly IStringLocalizer<CommonLocalizer> _localizer;
        private readonly ICustomDAL _dal;

        public CustomManager(IStringLocalizer<CommonLocalizer> localizer, ICustomDAL dal)
        {
            _localizer = localizer;
            _dal = dal;
        }

        public async Task<IResult> Contact(CreateOffer offer)
        {
            try
            {
                var response = await _dal.Contact(offer);
                return response;
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
                var response = await _dal.Delete(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Offer>>> GetAll(int page, int limit)
        {
            try
            {
                var response = await _dal.GetAll(page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Offer>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
        public async Task<IResult> MarkAsRead(Guid id)
        {
            try
            {
                var response = await _dal.MarkAsRead(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Reply(Guid id, string message)
        {
            try
            {
                var response = await _dal.Reply(id, message);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}