using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Clinics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Clinics.CRUD
{
    public class EFClinicUpdateDAL : ClinicAdapter
    {
        public EFClinicUpdateDAL(
            AppDbContext context,
            IStringLocalizer<ClinicAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }
        public override async Task<IResult> CheckAsUsed(Guid clinicId, Guid itemId)
        {
            var clinic = await _context.Set<Clinic>()
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == clinicId);

            if (clinic is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            var item = clinic.Orders.FirstOrDefault(c => c.Id == itemId);

            if (item is null)
                return new ErrorResult(_dalLocalizer["itemNotFound"], HttpStatusCode.NotFound);

            item.IsUsed = true;
            await _context.SaveChangesAsync();
            return new SuccessResult();
        }
    }
}