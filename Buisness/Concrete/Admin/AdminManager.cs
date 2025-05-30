using System.Net;
using Buisness.Abstract.Admin;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract.Admin;
using Entities.DTOs.Admin;
using Entities.DTOs.AuthDtos;
using Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete.Admin
{
    public class AdminManager : IAdminService
    {
        private readonly IStringLocalizer<Validator> _localizer;
        private readonly IAdminDAL _dal;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _axs;
        private readonly IWebHostEnvironment _env;
        public AdminManager(IStringLocalizer<Validator> localizer, IAdminDAL dal, Microsoft.AspNetCore.Http.IHttpContextAccessor axs, IWebHostEnvironment env)
        {
            _localizer = localizer;
            _dal = dal;
            _axs = axs;
            _env = env;
        }

        public async Task<IResult> AddAdmin(Create create)
        {
            try
            {
                string url = string.Empty;
                var requestContext = _axs?.HttpContext?.Request;
                string scheme = requestContext?.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? requestContext?.Scheme;
                string folderDir = Path.Combine(_env.WebRootPath, "assets", "Default");
                var baseUrl = $"{scheme}://{requestContext?.Host}/assets/Default/user.png";

                if (string.IsNullOrWhiteSpace(create.ProfileLogo))
                    create.ProfileLogo = baseUrl;

                var response = await _dal.AddAdmin(create);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
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
                return new ErrorDataResult<List<Get>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Login(string email, string password)
        {
            try
            {
                var response = await _dal.Login(email, password);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> ReloadAdmin(Guid id, string password, Superiority superiority)
        {
            try
            {
                var response = await _dal.ReloadAdmin(id, password,superiority);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}