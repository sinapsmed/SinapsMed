using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Services.Abstract;
using Entities.Concrete.Emails;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD
{
    public class EFWorkSpaceEmailCreateDAL : WorkSpaceEmailAdapter
    {
        private readonly IDAtaAccessService _service;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _email;
        public EFWorkSpaceEmailCreateDAL(AppDbContext context, IDAtaAccessService service, IConfiguration configuration, IEmailService email) : base(context)
        {
            _service = service;
            _configuration = configuration;
            _email = email;
        }

        public override async Task<IResult> AddAsync(string email, string password, string FullName)
        {
            byte[] salt = _configuration["Hashing:Default"]
                    .Split('-')
                    .Select(hex => Convert.ToByte(hex, 16))
                    .ToArray();

            var pasword = _service.HashPassword(password, salt);

            await _context.Emails.AddAsync(new WorkSpaceEmail
            {
                Email = email,
                PasswordHash = pasword,
                Title = FullName,
            });

            await _context.SaveChangesAsync();

            var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "Greeting.html");
            string fileContent = File.ReadAllText(url);

            fileContent = fileContent
                .Replace("{{fullName}}", FullName)
                .Replace("{{email}}", email);

            await _email.SendEmailAsync(email, "Xoş Gəlmisiniz!!!", fileContent);

            return new SuccessResult("Email added successfully");
        }
    }
}