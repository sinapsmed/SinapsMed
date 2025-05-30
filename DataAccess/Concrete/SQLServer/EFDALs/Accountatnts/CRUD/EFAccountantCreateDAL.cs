using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD;
using Entities.Concrete.AccountantEntities;
using Entities.Concrete.Emails;
using Entities.DTOs.AccountantDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Accountatnts.CRUD
{
    public class EFAccountantCreateDAL : AccountantAdapter
    {
        private readonly IConfiguration _config;
        private readonly EFWorkSpaceEmailCreateDAL _emailCreateDAL;
        public EFAccountantCreateDAL(AppDbContext context, IStringLocalizer<AccountantAdapter> dalLocalizer, IConfiguration config, EFWorkSpaceEmailCreateDAL emailCreateDAL) : base(context, dalLocalizer)
        {
            _config = config;
            _emailCreateDAL = emailCreateDAL;
        }

        public override async Task<IResult> AddAsync(Create create)
        {
            var changedEmail = create.FullName.Replace(" ", "").ToLower().ConverToSeo("az");

            var newAddress = $"{changedEmail}@{_config["Email:Domain"]}";

            var emailExists = await _context.Set<WorkSpaceEmail>().AnyAsync(c => c.Email == newAddress);

            if (emailExists)
                return new ErrorResult("Email already exists,please try another name or add a number to the name");

            var email = await _emailCreateDAL.AddAsync(newAddress, create.Password, create.FullName);
            if (!email.Success)
                return email;

            var workSpaceEmailId = await _context.Set<WorkSpaceEmail>().FirstOrDefaultAsync(c => c.Email == newAddress);
            if (workSpaceEmailId == null)
                return new ErrorResult("Email not found");

            var newAccountant = new Accountant
            {
                EmailId = workSpaceEmailId.Id,
            };

            await _context.Set<Accountant>().AddAsync(newAccountant);
            await _context.SaveChangesAsync();
            return new SuccessResult();
        }
    }
}