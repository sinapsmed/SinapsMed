using System.Net;
using Core.Entities;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Concrete.SQLServer.EFDALs.Common;
using DataAccess.Exceptions.Global;
using DataAccess.Services.Abstract;
using Entities.Common;
using Entities.Concrete.AccountantEntities;
using Entities.Concrete.BasketEntities;
using Entities.Concrete.Clinics;
using Entities.Concrete.Experts;
using Entities.Concrete.Staff;
using Entities.Concrete.UserEntities;
using Entities.DTOs.AuthDtos;
using Entities.Enums;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Auth;

public class EFAuthDAL : Manager, IAuthDAL
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IRedisCacheService _cache;
    private readonly AppDbContext _context;
    private readonly IDAtaAccessService _service;
    private readonly IEmailService _email;
    private readonly IStringLocalizer<EFAuthDAL> _dalLocalizer;
    private readonly IStringLocalizer<CommonLocalizer> _comLoclizer;
    private readonly IConfiguration _config;

    public EFAuthDAL(
        UserManager<AppUser> userManager,
        IEmailService email,
        IStringLocalizer<EFAuthDAL> dalLocalizer,
        IStringLocalizer<CommonLocalizer> comLoclizer,
        IDAtaAccessService service,
        AppDbContext context,
        IConfiguration config,
        IRedisCacheService cache)
    {
        _userManager = userManager;
        _email = email;
        _dalLocalizer = dalLocalizer;
        _comLoclizer = comLoclizer;
        _service = service;
        _context = context;
        _config = config;
        _cache = cache;
    }

    public async Task<IResult> ChangePassword(ChangePassword changePassword)
    {
        var user = await _userManager.FindByNameAsync(changePassword.UserName);

        if (user is null)
            return new ErrorResult(_dalLocalizer["neverUsed"], HttpStatusCode.BadRequest, "User Not Found");

        if (!await _userManager.CheckPasswordAsync(user, changePassword.OldPassword))
            return new ErrorResult(_dalLocalizer["wrongPassword"], HttpStatusCode.BadRequest, "Incorrect Password");

        var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);

        if (!result.Succeeded)
            return new ErrorResult(string.Join(", ", result.Errors.Select(c => c.Description)), HttpStatusCode.BadRequest);

        return new SuccessResult(_dalLocalizer["changed"], HttpStatusCode.OK);
    }

    public Task<IResult> CommonLogin(Superiority superiority, string loginId, string password)
    {
        string defaultHash = _config["Hashing:Default"];

        byte[] defaultSalt = defaultHash
                .Split('-')
                .Select(hex => Convert.ToByte(hex, 16))
                .ToArray();

        return superiority switch
        {
            Superiority.Clinic => ClinicLogin(loginId, password, defaultSalt),
            Superiority.Expert => ExpertLogin(loginId, password, defaultSalt),
            Superiority.Accountant => AccountantLogin(loginId, password, defaultSalt),
            Superiority.Staff => StaffLogin(loginId, password, defaultSalt),
            _ => throw new NotImplementedException()
        };
    }

    public async Task<IResult> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new ErrorResult(_dalLocalizer["neverUsed"], HttpStatusCode.BadRequest, "User Not Found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _cache.SetCacheValueAsync<string>($"{user.Email}-reset", token, TimeSpan.FromMinutes(10));

        var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "forgetpassword.html");

        string fileContent = File.ReadAllText(url);

        var resetLink = $"{_config["App:BaseLink"]}/reset?email={user.Email}&token={token}";

        fileContent = fileContent
            .Replace("{{username}}", user.FullName)
            .Replace("{{link}}", resetLink)
            .Replace("{{forgettext}}", _dalLocalizer["forgetText"])
            .Replace("{{thanks}}", _dalLocalizer["mailThanks"])
            .Replace("{{text}}", _dalLocalizer["mailText"]);

        await _email.SendEmailAsync(user.Email, _dalLocalizer["forgetPassword"], fileContent);

        return new SuccessResult(_dalLocalizer["send"], HttpStatusCode.OK);
    }

    public async Task<IDataResult<Token>> Login(Login login)
    {
        AppUser user = await _userManager.FindByEmailAsync(login.Email);
        if (user is null)
            return new ErrorDataResult<Token>(_dalLocalizer["unAuth"], HttpStatusCode.BadRequest, "User Not Found");

        if (!await _userManager.CheckPasswordAsync(user, login.Password))
            return new ErrorDataResult<Token>(_dalLocalizer["wrongPassword"], HttpStatusCode.BadRequest, $"Incorrect password for {user.FullName}");

        if (user.PhoneNumberConfirmed == false)
            return new ErrorDataResult<Token>(_dalLocalizer["notVerified"], HttpStatusCode.BadRequest, "Phone Number Not Verified");

        var roles = await _userManager.GetRolesAsync(user);

        string token = _service.GetToken(roles, user);

        return new SuccessDataResult<Token>(new Token { JWT = token }, HttpStatusCode.OK);
    }

    public async Task<IDataResult<Token>> OTP(string email, long otp)
    {
        var cache = await _cache.GetCacheValueAsync<OtpVeirfication>($"{email}-otp");

        if (cache is default(OtpVeirfication) || cache is null)
            return new ErrorDataResult<Token>(_dalLocalizer["expiredOtp"]);

        if (cache.Otp != otp)
            return new ErrorDataResult<Token>(_dalLocalizer["wrongOtp"]);

        return cache.Superiority switch
        {
            Superiority.Expert => await ExpertOtpVerification(email),
            Superiority.Clinic => await ClinicOtpVerification(email),
            Superiority.Admin => await AdminOtpVerification(email),
            Superiority.Accountant => await AccountantOtpVerification(email),
            Superiority.Staff => await StaffOtpVerification(email),
            _ => throw new NotImplementedException()
        };
    }

    public async Task<IDataResult<DateTime>> PhoneOtpAgain(string phone, string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == email);

        if (user is null)
            return new ErrorDataResult<DateTime>(_dalLocalizer["neverUsed"], HttpStatusCode.BadRequest, "User Not Found");

        var data = await _cache.GetCacheValueAsync<OTP>(user.PhoneNumber);

        if (data is not default(OTP))
        {
            return new SuccessDataResult<DateTime>(data.Expired, _dalLocalizer["notExpired"], HttpStatusCode.OK);
        }

        Random random = new();
        var num = random.NextInt64(100000, 999999);
        var otp = new OTP { Code = num, Expired = DateTime.UtcNow.AddMinutes(15), IsVerificated = false };

        await _cache.SetCacheValueAsync<OTP>(user.PhoneNumber, otp, TimeSpan.FromMinutes(15));

        var smsResult = await _service.SendVerificationSMSAsync(phone.Trim('+'), _dalLocalizer["verificationBody", num.ToString("### ###"), 10]);

        if (!smsResult)
            return new ErrorDataResult<DateTime>("Doğrulama kodu göndərilə bilmədi");

        return new SuccessDataResult<DateTime>(otp.Expired, _dalLocalizer["send"], HttpStatusCode.OK);
    }

    public async Task<IDataResult<DateTime>> Register(Register register)
    {
        using (var context = new AppDbContext())
        {
            var transiction = context.Database.BeginTransaction();
            bool isSucces = false;
            AppUser user = new();
            string password = _service.GeneratePasswrod();
            do
            {
                user = new()
                {
                    FullName = register.Name + " " + register.SurName,
                    Gender = register.Gender,
                    Email = register.Mail,
                    PhoneNumber = register.PhoneNumber,
                    EmailConfirmed = true,
                    UserName = Guid.NewGuid().ToString(),
                    ImageUrl = register.ImageUrl,
                    UnicalKey = AuthService.GenerateUniqueUnicalKey(_context)
                };
                IdentityResult result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    return new ErrorDataResult<DateTime>(message: string.Join(", ", result.Errors.Select(c => c.Description)), statusCode: HttpStatusCode.BadRequest);
                await _userManager.AddToRoleAsync(user, "User");

                try
                {
                    transiction.Commit();
                    await context.SaveChangesAsync();

                    isSucces = true;
                }
                catch (DbUpdateException)
                {
                    user.UnicalKey = AuthService.GenerateUniqueUnicalKey(_context);
                }
                catch (Exception ex)
                {
                    return new ErrorDataResult<DateTime>(message: _comLoclizer["ex"], statusCode: HttpStatusCode.BadRequest, ex);
                }

            } while (!isSucces);

            Random random = new();

            var num = random.NextInt64(100000, 999999);
            var expired = DateTime.UtcNow.AddMinutes(10);
            await _cache.SetCacheValueAsync<OTP>(user.PhoneNumber, new OTP { Code = num, Expired = expired, IsVerificated = false }, TimeSpan.FromMinutes(10));
            var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "password.html");

            string fileContent = File.ReadAllText(url);

            fileContent = fileContent
                .Replace("{{username}}", user.FullName)
                .Replace("{{password}}", $"{password}");

            var smsResult = await _service.SendVerificationSMSAsync(register.PhoneNumber.Trim('+'), _dalLocalizer["verificationBody", num.ToString("### ###"), 10]);

            if (!smsResult)
                return new ErrorDataResult<DateTime>("Doğrulama kodu göndərilə bilmədi");

            await _email.SendEmailAsync(user.Email, _dalLocalizer["mail"], fileContent);

            return new SuccessDataResult<DateTime>(data: expired, message: _dalLocalizer["registerSucces"], statusCode: HttpStatusCode.OK);
        }
    }

    public async Task<IResult> ResetPassword(string email, string token, string resetPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new ErrorResult(_dalLocalizer["neverUsed"], HttpStatusCode.BadRequest, "User Not Found");

        var data = await _cache.GetCacheValueAsync<string>($"{user.Email}-reset");

        if (data is default(string))
            return new ErrorResult(_dalLocalizer["expiredToken"], HttpStatusCode.BadRequest, "Token Expired");

        var result = await _userManager.ResetPasswordAsync(user, data, resetPassword);

        if (!result.Succeeded)
            return new ErrorResult(_dalLocalizer["identity"], HttpStatusCode.BadRequest, string.Join(", ", result.Errors.Select(c => c.Description)));

        return new SuccessResult(_dalLocalizer["changed"], HttpStatusCode.OK);
    }

    public async Task<IResult> UpdateUser(UserInfo updateUser)
    {
        var user = await _userManager.FindByEmailAsync(updateUser.Email);

        if (user is null)
            return new ErrorResult(_dalLocalizer["neverUsed"], HttpStatusCode.BadRequest, "User Not Found");

        if (updateUser.Name != null || updateUser.SurName != null)
            user.FullName = $"{updateUser.Name} {updateUser.SurName}";

        user.PhoneNumber = updateUser.PhoneNumber ?? user.PhoneNumber;

        if (updateUser.DateOfBrith.HasValue)
        {
            var dob = updateUser.DateOfBrith.Value;
            user.DateOfBrith = DateTime.SpecifyKind(dob, DateTimeKind.Utc);
        }

        user.Gender = updateUser.Gender;
        user.Country = updateUser.Country ?? user.Country;
        user.City = updateUser.City ?? user.City;

        user.ImageUrl = updateUser.ImageUrl ?? user.ImageUrl;

        await _userManager.UpdateAsync(user);

        return new SuccessResult(_dalLocalizer["updated"], HttpStatusCode.OK);
    }

    public async Task<IDataResult<UserInfo>> UserInfo(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId); ;

        if (user is null)
            return new ErrorDataResult<UserInfo>(_dalLocalizer["unAuth"], HttpStatusCode.BadRequest, "User Not Found");

        var data = new UserInfo
        {
            Email = user.Email,
            Name = user.FullName.Split(' ')[0],
            City = user.City,
            Country = user.Country,
            ImageUrl = user.ImageUrl,
            SurName = user.FullName.Split(' ')[1],
            PhoneNumber = user.PhoneNumber,
            UnicalKey = user.UnicalKey,
            DateOfBrith = user.DateOfBrith,
            Gender = user.Gender
        };

        return new SuccessDataResult<UserInfo>(data, HttpStatusCode.OK);
    }

    public async Task<IResult> VerifyPhone(string mail, string otp)
    {
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == mail);

        if (user is null)
            return new ErrorResult(_dalLocalizer["neverUsed"], HttpStatusCode.BadRequest, "User Not Found");

        var data = await _cache.GetCacheValueAsync<OTP>(user.PhoneNumber);

        if (data is default(OTP))
            return new ErrorResult(_dalLocalizer["expiredOtp"], HttpStatusCode.BadRequest, "OTP Code Expired");

        if (data.Code != Convert.ToInt32(otp))
            return new ErrorResult(_dalLocalizer["incorrectOtp"], HttpStatusCode.BadRequest, "Incorrect OTP Code");

        user.PhoneNumberConfirmed = true;
        await _context.SaveChangesAsync();

        return new SuccessResult(_dalLocalizer["verified"], HttpStatusCode.OK);
    }

    private async Task<IResult> ClinicLogin(string loginId, string password, byte[] salt)
    {
        string unicalKey = loginId.Trim().ToLower();
        System.Console.WriteLine(unicalKey);
        if (loginId.Contains('@'))
            unicalKey = loginId.Split('@')[0];

        System.Console.WriteLine(loginId);
        var clinic = _context.Set<Clinic>()
            .Include(c => c.Email)
            .FirstOrDefault(c => c.UnicalKey.ToLower() == unicalKey);

        if (clinic is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Clinic Not Found");

        if (_service.HashPassword(password, salt) != clinic.Email.PasswordHash)
            return new ErrorDataResult<Token>(_dalLocalizer["wrongPassword"], HttpStatusCode.BadRequest, "Incorrect Password");


        Random random = new();
        var num = random.NextInt64(100000, 999999);

        await _cache.SetCacheValueAsync($"{loginId}-otp",
         new OtpVeirfication
         {
             Otp = num,
             Superiority = Superiority.Clinic
         },
         TimeSpan.FromMinutes(5));

        var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "otp.html");

        string fileContent = File.ReadAllText(url);

        fileContent = fileContent
            .Replace("{{code}}", num.ToString("### ###"))
            .Replace("{{min}}", "5");

        await _email.SendEmailAsync(clinic.Email.Email, "Doğrulama Kodunuz", fileContent);

        return new SuccessResult();
    }

    private async Task<IResult> StaffLogin(string loginId, string password, byte[] salt)
    {
        var clinic = _context.Set<Support>()
            .Include(c => c.WorkSpaceEmail)
            .FirstOrDefault(c => c.WorkSpaceEmail.Email.ToLower() == loginId.ToLower());

        if (clinic is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Staff Not Found");

        if (_service.HashPassword(password, salt) != clinic.WorkSpaceEmail.PasswordHash)
            return new ErrorDataResult<Token>(_dalLocalizer["wrongPassword"], HttpStatusCode.BadRequest, "Incorrect Password");


        Random random = new();
        var num = random.NextInt64(100000, 999999);

        await _cache.SetCacheValueAsync($"{clinic.WorkSpaceEmail.Email}-otp",
         new OtpVeirfication
         {
             Otp = num,
             Superiority = Superiority.Staff
         },
         TimeSpan.FromMinutes(5));

        var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "otp.html");

        string fileContent = File.ReadAllText(url);

        fileContent = fileContent
            .Replace("{{code}}", num.ToString("### ###"))
            .Replace("{{min}}", "5");

        await _email.SendEmailAsync(clinic.WorkSpaceEmail.Email, "Doğrulama Kodunuz", fileContent);

        return new SuccessResult();
    }

    private async Task<IResult> AccountantLogin(string loginId, string password, byte[] salt)
    {
        var accountant = _context.Set<Accountant>()
            .Include(c => c.Email)
            .FirstOrDefault(c => c.Email.Email == loginId);

        if (accountant is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Accountant Not Found");

        if (_service.HashPassword(password, salt) != accountant.Email.PasswordHash)
            return new ErrorDataResult<Token>(_dalLocalizer["wrongPassword"], HttpStatusCode.BadRequest, "Incorrect Password");

        Random random = new();
        var num = random.NextInt64(100000, 999999);

        await _cache.SetCacheValueAsync($"{accountant.Email.Email}-otp",
         new OtpVeirfication
         {
             Otp = num,
             Superiority = Superiority.Accountant
         },
         TimeSpan.FromMinutes(2));

        var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "otp.html");

        string fileContent = File.ReadAllText(url);

        fileContent = fileContent
            .Replace("{{code}}", num.ToString("### ###"))
            .Replace("{{min}}", "2");

        await _email.SendEmailAsync(accountant.Email.Email, "Doğrulama Kodunuz", fileContent);

        return new SuccessResult();
    }

    private async Task<IResult> ExpertLogin(string loginId, string password, byte[] salt)
    {
        var expert = _context.Set<Expert>().FirstOrDefault(c => c.Email == loginId);

        if (expert is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Expert Not Found");

        if (_service.HashPassword(password, salt) != expert.PasswordHash)
            return new ErrorDataResult<Token>(_dalLocalizer["wrongPassword"], HttpStatusCode.BadRequest, "Incorrect Password");

        Random random = new();
        var num = random.NextInt64(100000, 999999);

        await _cache.SetCacheValueAsync($"{expert.Email}-otp",
         new OtpVeirfication
         {
             Otp = num,
             Superiority = Superiority.Expert
         },
         TimeSpan.FromMinutes(2));
        var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "otp.html");

        string fileContent = File.ReadAllText(url);

        fileContent = fileContent
            .Replace("{{code}}", num.ToString("### ###"))
            .Replace("{{min}}", "2");

        await _email.SendEmailAsync(expert.Email, "Doğrulama Kodunuz", fileContent);

        return new SuccessResult();
    }

    private async Task<IDataResult<Token>> ExpertOtpVerification(string email)
    {
        var expert = await _context.Set<Expert>().FirstOrDefaultAsync(c => c.Email == email);

        if (expert is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Expert Not Found");

        var token = new Token
        {
            JWT = _service.ExpertToken(expert)
        };

        return new SuccessDataResult<Token>(token);
    }

    private async Task<IDataResult<Token>> StaffOtpVerification(string email)
    {
        var expert = await _context.Set<Support>()
            .Include(c => c.WorkSpaceEmail)
            .FirstOrDefaultAsync(c => c.WorkSpaceEmail.Email.ToLower() == email.ToLower());

        if (expert is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Staff Not Found");

        var token = new Token
        {
            JWT = _service.GetToken(new List<string> { "Staff" }, expert)
        };

        return new SuccessDataResult<Token>(token);
    }

    private async Task<IDataResult<Token>> ClinicOtpVerification(string email)
    {
        string unicalKey = email.Trim().ToLower();
        if (email.Contains('@'))
            unicalKey = email.Split('@')[0];

        var clinic = await _context.Set<Clinic>()
            .Include(c => c.Email)
            .FirstOrDefaultAsync(c => c.UnicalKey.ToLower() == unicalKey);

        if (clinic is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Clinic Not Found");

        var token = new Token
        {
            JWT = _service.GetToken(new List<string> { "Clinic" }, clinic)
        };

        return new SuccessDataResult<Token>(token);
    }

    private async Task<IDataResult<Token>> AccountantOtpVerification(string email)
    {
        var accountant = await _context.Set<Accountant>()
            .Include(c => c.Email)
            .FirstOrDefaultAsync(c => c.Email.Email == email.ToLower());

        if (accountant is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.BadRequest, "Accountant Not Found");

        var token = new Token
        {
            JWT = _service.GetToken(new List<string> { "Accountant" }, accountant)
        };

        return new SuccessDataResult<Token>(token);
    }

    private async Task<IDataResult<Token>> AdminOtpVerification(string email)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

        if (admin is null)
            return new ErrorDataResult<Token>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, new NotFoundException(email));


        var token = new Token
        {
            JWT = _service.GetToken(new List<string> { "Admin" }, admin)
        };

        return new SuccessDataResult<Token>(token);
    }

    public async Task<IDataResult<DateTime>> ChangePhone(string phone, string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == email);

        if (user is null)
            return new ErrorDataResult<DateTime>(_dalLocalizer["emailNeverUsed"], HttpStatusCode.BadRequest, "User Not Found");

        var data = await _cache.GetCacheValueAsync<OTP>(user.PhoneNumber);

        if (data is not default(OTP))
        {
            return new SuccessDataResult<DateTime>(data.Expired, _dalLocalizer["notExpired"], HttpStatusCode.OK);
        }

        Random random = new();
        var num = random.NextInt64(100000, 999999);
        var otp = new OTP { Code = num, Expired = DateTime.UtcNow.AddMinutes(10), IsVerificated = false };

        await _cache.SetCacheValueAsync<OTP>(user.PhoneNumber, otp, TimeSpan.FromMinutes(10));

        await _email.SendEmailAsync(user.Email, _dalLocalizer["mail"], _dalLocalizer["mailPassword", $"Phone Otp Is : {num}"]);

        // await _service.SendVerificationSMSAsync(phone.Trim('+'), _dalLocalizer["verificationBody", num.ToString("### ###"), 25]);

        return new SuccessDataResult<DateTime>(otp.Expired, _dalLocalizer["send"], HttpStatusCode.OK);
    }
}
