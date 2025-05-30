using Core.Entities;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Services.Abstract;
using Entities.Concrete.Emails;
using Entities.DTOs.AuthDtos;
using Entities.DTOs.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD
{
    public class EFWorkSpaceEmailCustomDAL : WorkSpaceEmailAdapter
    {
        private readonly IDAtaAccessService _service;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public EFWorkSpaceEmailCustomDAL(AppDbContext context, IDAtaAccessService service, IConfiguration configuration, IEmailService emailService) : base(context)
        {
            _service = service;
            _configuration = configuration;
            _emailService = emailService;
        }

        public override async Task<IDataResult<Token>> CheckPassWordAsync(string email, string pasword)
        {
            var adress = await _context.Set<WorkSpaceEmail>()
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

            string hexString = _configuration["Hashing:Default"];

            byte[] salt = hexString
                    .Split('-')
                    .Select(hex => Convert.ToByte(hex, 16))
                    .ToArray();

            if (adress.PasswordHash == _service.HashPassword(pasword, salt))
            {

                var token = _service.WorkSpaceToken(adress.Email, adress.Id);

                return new SuccessDataResult<Token>(token);
            }
            else
                return new ErrorDataResult<Token>("Password is not correct");
        }

        public override async Task<IResult> SendEmailAsync(Guid workspaceEmailId, string to, string title, string content)
        {
            var email = await _context.Set<WorkSpaceEmail>()
                .FirstOrDefaultAsync(c => c.Id == workspaceEmailId);

            if (email == null)
                return new ErrorResult("Email not found");

            var source = $"\"{email.Title}\" <{email.Email}>";

            return await _emailService.SendEmailAsync(to, title, content, source);
        }

    }
}