using System.Net;
using Buisness.Abstract;
using Buisness.Services.Static;
using Buisness.Validation.Auth;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using Entities.DTOs.AuthDtos;
using Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class AuthManager : Manager, IAuthService
    {
        private readonly IAuthDAL _dal;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _axs;
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<Validator> _loclalizer;
        public AuthManager(
            IAuthDAL dal,
            IStringLocalizer<Validator> loclalizer,
            Microsoft.AspNetCore.Http.IHttpContextAccessor axs,
            IWebHostEnvironment env)
        {
            _dal = dal;
            _loclalizer = loclalizer;
            _axs = axs;
            _env = env;
        }

        public async Task<IResult> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                var response = await _dal.ChangePassword(changePassword);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<DateTime>> ChangePhone(string phone, string email)
        {
            try
            {
                var response = await _dal.ChangePhone(phone, email);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<DateTime>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> CommonLogin(Superiority superiority, string loginId, string password)
        {
            try
            {
                var response = await _dal.CommonLogin(superiority, loginId, password);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
        public async Task<IResult> ForgotPassword(string email)
        {
            try
            {
                var response = await _dal.ForgotPassword(email);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Token>> Login(Login login)
        {
            try
            {
                var result = await GenericDataValidator<Login, Token, LoginValidator>.ValidateDataAsync(login, new Token(), new LoginValidator(_loclalizer));
                if (!result.Success)
                    return result;
                var response = await _dal.Login(login);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Token>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Token>> OTP(string email, long otp)
        {
            try
            {
                var response = await _dal.OTP(email, otp);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Token>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<DateTime>> PhoneOtpAgain(string phone, string email)
        {
            try
            {
                var response = await _dal.PhoneOtpAgain(phone, email);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<DateTime>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<DateTime>> Register(Register register)
        {
            try
            {
                string url = string.Empty;
                var requestContext = _axs?.HttpContext?.Request;
                string scheme = requestContext?.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? requestContext?.Scheme;
                string folderDir = Path.Combine(_env.WebRootPath, "assets", "Default");
                var baseUrl = $"{scheme}://{requestContext?.Host}/assets/Default/user.png";
                register.ImageUrl = baseUrl;
                var result = await GenericValidator<Register, RegisterValidator>.ValidateAsync(register, new RegisterValidator(_loclalizer));
                if (!result.Success)
                    return new ErrorDataResult<DateTime>(result.Message, result.ErrorMessage);
                var response = await _dal.Register(register);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<DateTime>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> ResetPassword(string email, string token, string resetPassword)
        {
            try
            {
                var response = await _dal.ResetPassword(email, token, resetPassword);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdateUser(UserInfo updateUser)
        {
            try
            {
                var response = await _dal.UpdateUser(updateUser);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<UserInfo>> UserInfo(string userId)
        {
            try
            {
                var response = await _dal.UserInfo(userId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserInfo>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> VerifyPhone(string phone, string otp)
        {
            try
            {
                var response = await _dal.VerifyPhone(phone, otp);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserInfo>(_loclalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }

}

