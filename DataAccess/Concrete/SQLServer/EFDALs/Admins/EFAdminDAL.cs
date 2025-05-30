using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Core.DataAccess;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Abstract.Admin;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using DataAccess.Services.Abstract;
using Entities.Concrete.Admin;
using Entities.Concrete.Emails;
using Entities.Concrete.Experts;
using Entities.DTOs.Admin;
using Entities.DTOs.AuthDtos;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Admins
{
    public class EFAdminDAL : IAdminDAL
    {
        private readonly IRedisCacheService _cache;
        private readonly IRepositoryBase<Admin, Get, AppDbContext> _repo;
        private readonly IStringLocalizer<EFAdminDAL> _localizer;
        private readonly IConfiguration _config;
        private readonly IEmailService _email;
        private readonly IDAtaAccessService _dataAcces;

        public EFAdminDAL(IRepositoryBase<Admin, Get, AppDbContext> repo, IStringLocalizer<EFAdminDAL> localizer, IDAtaAccessService dataAcces, IConfiguration config, IEmailService email, IRedisCacheService cache)
        {
            _repo = repo;
            _localizer = localizer;
            _dataAcces = dataAcces;
            _config = config;
            _email = email;
            _cache = cache;
        }

        public async Task<IResult> AddAdmin(Create create)
        {
            using (var context = new AppDbContext())
            {
                if (await context.Admins.AnyAsync(c => c.Email.ToLower() == create.Email.ToLower()))
                    return new ErrorResult(_localizer["alreadyExsist"], HttpStatusCode.BadRequest, new AlreadyUsedException(create.Email));

                var passwordCheck = _dataAcces.CheckPasswordRequirements(create.Password);

                if (!passwordCheck.Success)
                    return passwordCheck;

                Admin admin = create.Map<Admin, Create>();

                string hexString = _config["Hashing:Default"];

                byte[] salt = hexString
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();

                admin.Password = _dataAcces.HashPassword(create.Password, salt);

                return await _repo.AddAsync(admin, context);
            }
        }

        public async Task<IDataResult<List<Get>>> GetAll()
        {
            using (var context = new AppDbContext())
            {
                var query = context.Set<Admin>();

                Expression<Func<Admin, Get>> Get()
                {
                    return c => new Get
                    {
                        Id = c.Id,
                        Email = c.Email,
                        Name = c.Name,
                        WrongPassword = c.WrongPassword,
                        Image = c.ProfileLogo
                    };
                }

                return await _repo.GetAllAsync(query, selector: Get());
            }
        }

        public async Task<IResult> Login(string email, string password)
        {
            using (var context = new AppDbContext())
            {
                var admin = await context.Admins.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

                if (admin is null)
                    return new ErrorResult(_localizer["notFound"], HttpStatusCode.NotFound, new NotFoundException(email));

                if (admin.WrongPassword == 5)
                    return new ErrorResult(_localizer["limitHitted"], HttpStatusCode.NotFound);

                string hexString = _config["Hashing:Default"];

                byte[] salt = hexString
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();


                if (_dataAcces.HashPassword(password, salt) != admin.Password)
                {
                    admin.WrongPassword++;

                    await context.SaveChangesAsync();

                    return new ErrorResult(_localizer["wrongPassword", 5 - admin.WrongPassword], HttpStatusCode.NotFound);
                }
                Random random = new();
                var num = random.NextInt64(100000, 999999);

                await _cache.SetCacheValueAsync($"{admin.Email}-otp"
                ,
                new OtpVeirfication
                {
                    Otp = num,
                    Superiority = Superiority.Admin
                }
                ,
                 TimeSpan.FromMinutes(2));


                var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "otp.html");

                string fileContent = File.ReadAllText(url);

                fileContent = fileContent
                    .Replace("{{code}}", num.ToString("### ###"))
                    .Replace("{{min}}", "2");

                await _email.SendEmailAsync(admin.Email, "Doğrulama Kodunuz", fileContent);

                return new SuccessResult();
            }
        }

        public async Task<IResult> ReloadAdmin(Guid id, string password, Superiority superiority)
        {
            return superiority switch
            {
                Superiority.Admin => await AdminReload(id, password),
                Superiority.Expert => await ExpertReload(id, password),
                Superiority.Staff => await StaffReload(id, password),
                Superiority.Clinic => await ClinicReload(id, password),
                Superiority.Accountant => await AccountantReload(id, password),
                _ => throw new NotImplementedException()
            };
        }
        private async Task<IResult> AccountantReload(Guid id, string password)
        {
            using (var context = new AppDbContext())
            {
                var accountant = await context.Accountants.Include(c => c.Email).FirstOrDefaultAsync(c => c.Id == id);

                if (accountant is null)
                    return new ErrorDataResult<Token>(_localizer["accountantNotFound"], HttpStatusCode.NotFound, new NotFoundException(id.ToString()));

                var passwordCheck = _dataAcces.CheckPasswordRequirements(password);

                if (!passwordCheck.Success)
                    return passwordCheck;

                string hexString = _config["Hashing:Default"];

                byte[] salt = hexString
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();

                accountant.Email.PasswordHash = _dataAcces.HashPassword(password, salt);

                await context.SaveChangesAsync();
                return new SuccessResult();
            }
        }
        private async Task<IResult> ClinicReload(Guid id, string password)
        {
            using (var context = new AppDbContext())
            {
                var support = await context.Clinics.Include(c => c.Email).FirstOrDefaultAsync(c => c.Id == id);

                if (support is null)
                    return new ErrorDataResult<Token>(_localizer["clinicNotFound"], HttpStatusCode.NotFound, new NotFoundException(id.ToString()));

                var passwordCheck = _dataAcces.CheckPasswordRequirements(password);

                if (!passwordCheck.Success)
                    return passwordCheck;

                string hexString = _config["Hashing:Default"];

                byte[] salt = hexString
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();

                support.Email.PasswordHash = _dataAcces.HashPassword(password, salt);

                await context.SaveChangesAsync();
                return new SuccessResult();
            }
        }
        private async Task<IResult> StaffReload(Guid id, string password)
        {
            using (var context = new AppDbContext())
            {
                var support = await context.Supports.Include(c => c.WorkSpaceEmail).FirstOrDefaultAsync(c => c.Id == id);

                if (support is null)
                    return new ErrorDataResult<Token>(_localizer["staffNotFound"], HttpStatusCode.NotFound, new NotFoundException(id.ToString()));

                var passwordCheck = _dataAcces.CheckPasswordRequirements(password);

                if (!passwordCheck.Success)
                    return passwordCheck;

                string hexString = _config["Hashing:Default"];

                byte[] salt = hexString
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();

                support.WorkSpaceEmail.PasswordHash = _dataAcces.HashPassword(password, salt);

                await context.SaveChangesAsync();
                return new SuccessResult();
            }
        }
        private async Task<IResult> ExpertReload(Guid id, string password)
        {
            using (var context = new AppDbContext())
            {
                var expert = await context.Experts.FirstOrDefaultAsync(c => c.Id == id);

                if (expert is null)
                    return new ErrorDataResult<Token>(_localizer["expertNotFound"], HttpStatusCode.NotFound, new NotFoundException(id.ToString()));

                var passwordCheck = _dataAcces.CheckPasswordRequirements(password);

                if (!passwordCheck.Success)
                    return passwordCheck;

                string hexString = _config["Hashing:Default"];

                byte[] salt = hexString
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();

                expert.PasswordHash = _dataAcces.HashPassword(password, salt);

                await context.SaveChangesAsync();
                return new SuccessResult();
            }
        }
        private async Task<IResult> AdminReload(Guid id, string password)
        {
            using (var context = new AppDbContext())
            {
                var admin = await context.Admins.FirstOrDefaultAsync(c => c.Id == id);

                if (admin is null)
                    return new ErrorDataResult<Token>(_localizer["notFound"], HttpStatusCode.NotFound, new NotFoundException(id.ToString()));

                var passwordCheck = _dataAcces.CheckPasswordRequirements(password);

                if (!passwordCheck.Success)
                    return passwordCheck;

                string hexString = _config["Hashing:Default"];

                byte[] salt = hexString
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();

                admin.WrongPassword = 0;
                admin.Password = _dataAcces.HashPassword(password, salt);
                await context.SaveChangesAsync();
                return new SuccessResult();
            }
        }
    }
}