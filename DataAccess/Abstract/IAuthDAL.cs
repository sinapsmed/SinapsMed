using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AuthDtos;
using Entities.Enums;

namespace DataAccess.Abstract;

public interface IAuthDAL : IService
{
    Task<IDataResult<DateTime>> Register(Register register);
    Task<IDataResult<Token>> Login(Login login);
    Task<IDataResult<UserInfo>> UserInfo(string userId);
    Task<IResult> VerifyPhone(string phone, string otp);
    Task<IDataResult<DateTime>> PhoneOtpAgain(string phone, string email);
    Task<IResult> ChangePassword(ChangePassword changePassword);
    Task<IResult> ForgotPassword(string email);
    Task<IResult> ResetPassword(string email, string token, string resetPassword);
    Task<IResult> UpdateUser(UserInfo updateUser);
    Task<IResult> CommonLogin(Superiority superiority, string loginId, string password);
    Task<IDataResult<Token>> OTP(string email, long otp);
    Task<IDataResult<DateTime>> ChangePhone(string phone, string email);

}