using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.CuponCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Cupons.CRUD
{
    public class EFCuponDeleteDAL : EFCuponCodeAdapter
    {
        public EFCuponDeleteDAL(AppDbContext context, IStringLocalizer<EFCuponCodeAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }

        public override async Task<IResult> Delete(Guid id)
        {
            var querry = await _context.Set<Cupon>()
                .Include(c => c.UsedCupons)
                .Include(c => c.SpesficCuponUsers)
                .Include(c => c.SpesficServiceCupons)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (querry is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            if (querry.UsedCupons.Count() > 0)
                return new ErrorResult(_dalLocalizer["usedCuponCantDelte"], HttpStatusCode.BadRequest);

            if (querry.IsActive)
                return new ErrorResult(_dalLocalizer["activeCupon"], HttpStatusCode.BadRequest);

            _context.Set<Cupon>().Remove(querry);

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
    }
}