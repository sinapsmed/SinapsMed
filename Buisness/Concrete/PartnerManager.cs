using System.Net;
using Buisness.Abstract;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using Entities.DTOs.PartnerDtos;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class PartnerManager :Manager, IPartnerService
    {
        private readonly IStringLocalizer<CommonLocalizer> _commonLocalizer;
        private readonly IPartnerDAL _dal;

        public PartnerManager(IStringLocalizer<CommonLocalizer> commonLocalizer, IPartnerDAL dal)
        {
            _commonLocalizer = commonLocalizer;
            _dal = dal;
        }

        public async Task<IResult> Create(Create create)
        {
            try
            {
                var response = await _dal.Create(create);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
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
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<Get>>> GetAll()
        {
            try
            {
                var response = await _dal.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Get>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(Update update)
        {
            try
            {
                var response = await _dal.Update(update);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}