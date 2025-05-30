using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Services.Abstract;
using Entities.Concrete.AccountantEntities;
using Entities.DTOs.AccountantDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Accountatnts.CRUD
{
    public class EFAccountantUpdateDAL : AccountantAdapter
    {
        private readonly IDAtaAccessService _service;
        private readonly IConfiguration _configuration;
        public EFAccountantUpdateDAL(AppDbContext context, IStringLocalizer<AccountantAdapter> dalLocalizer, IDAtaAccessService service, IConfiguration configuration) : base(context, dalLocalizer)
        {
            _service = service;
            _configuration = configuration;
        }

        public override async Task<IResult> UpdateAsync(Update update)
        {
            var accountant = await _context
                .Set<Accountant>()
                .Include(c => c.Email)
                .FirstOrDefaultAsync(c => c.Id == update.Id);

            if (accountant == null)
                return new ErrorResult(_dalLocalizer["AccountantNotFound"]);

            if (update.FullName != null)
                accountant.Email.Title = update.FullName;

            if (update.NewPassword != null)
            {
                byte[] salt = _configuration["Hashing:Default"]
                        .Split('-')
                        .Select(hex => Convert.ToByte(hex, 16))
                        .ToArray();

                var pasword = _service.HashPassword(update.NewPassword, salt);

                if (update.ReqFrom != null && update.ReqFrom.Superiority is Entities.Enums.Superiority.Admin)
                {
                    accountant.Email.PasswordHash = pasword;
                }
                else
                {
                    if (update.OldPassword != null)
                    {
                        var oldPassword = _service.HashPassword(update.OldPassword, salt);

                        if (oldPassword != accountant.Email.PasswordHash)
                            return new ErrorResult(_dalLocalizer["oldPasswordNotMatch"]);

                        accountant.Email.PasswordHash = pasword;
                    }
                    else
                    {
                        return new ErrorResult(_dalLocalizer["oldPasswordRequired"]);
                    }
                }
            }

            _context.Set<Accountant>().Update(accountant);
            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
    }
}