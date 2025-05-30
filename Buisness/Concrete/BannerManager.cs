using System.Net;
using Buisness.Abstract;
using Buisness.Services.Static;
using Buisness.Validation.Auth;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using Entities.DTOs.BannerDtos;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class BannerManager : Manager, IBannerService
    {
        private readonly IStringLocalizer<Validator> _loclalizer;
        private readonly IStringLocalizer<CommonLocalizer> _commonLocalizer;
        private readonly IBannerDAL _banner;

        public BannerManager(
            IStringLocalizer<CommonLocalizer> commonLocalizer,
            IBannerDAL banner,
            IStringLocalizer<Validator> loclalizer)
        {
            _commonLocalizer = commonLocalizer;
            _banner = banner;
            _loclalizer = loclalizer;
        }

        public async Task<IResult> Create(Create banner, string userName)
        {
            try
            {
                var result = await GenericValidator<Create, CreateValidator>.ValidateAsync(banner, new CreateValidator(_loclalizer));
                if (!result.Success)
                    return result;
                var response = await _banner.Create(banner, userName);
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
                var response = await _banner.Delete(id);
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
                var response = await _banner.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Get>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(Update update, string userName)
        {
            try
            {
                foreach (var langCreate in update.Languages)
                {
                    var langresult = await GenericValidator<LangUpdate, UpdateLangValidator>.ValidateAsync(langCreate, new UpdateLangValidator(_loclalizer));
                    if (!langresult.Success)
                        return langresult;
                }
                var response = await _banner.Update(update, userName);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Update>> UpdateData(Guid id)
        {
            try
            {
                var response = await _banner.UpdateData(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Update>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}