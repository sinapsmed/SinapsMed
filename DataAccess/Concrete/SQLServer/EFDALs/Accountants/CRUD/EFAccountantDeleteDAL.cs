using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.AccountantEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Accountatnts.CRUD
{
    public class EFAccountantDeleteDAL : AccountantAdapter
    {
        public EFAccountantDeleteDAL(AppDbContext context, IStringLocalizer<AccountantAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }

        public override async Task<IResult> DeleteAsync(Guid id)
        {
            var accountant = await _context
                .Set<Accountant>()
                .Include(c => c.Email)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (accountant == null)
                return new ErrorResult(_dalLocalizer["AccountantNotFound"]);

            _context.Emails.Remove(accountant.Email);

            _context.Set<Accountant>().Remove(accountant);

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
    }
}