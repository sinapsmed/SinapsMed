using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.CuponCodes;
using Entities.DTOs.CuponDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Cupons.CRUD
{
    public class EFCuponUpdateDAL : EFCuponCodeAdapter
    {
        public EFCuponUpdateDAL(AppDbContext context, IStringLocalizer<EFCuponCodeAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }

        public override async Task<IResult> UseCupon(string userId, string code, double amount)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);

            if (user is null)
                return new ErrorResult(_dalLocalizer["userNotFound"], HttpStatusCode.NotFound);

            var cupon = await _context.Set<Cupon>()
                .Include(c => c.UsedCupons)
                .FirstOrDefaultAsync(c => c.Code == code);

            if (cupon is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            await _context.Set<CuponUsing>().AddAsync(new CuponUsing
            {
                Amount = amount,
                CuponId = cupon.Id,
                UserId = userId,
                UsedAt = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync();
            return new SuccessResult();
        }

        public override async Task<IResult> Update(Update update)
        {
            var querry = await _context.Set<Cupon>()
                .FirstOrDefaultAsync(c => c.Id == update.Id);

            if (querry is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            if (querry.UsedCupons.Count() > update.UseLimit)
                return new ErrorResult(_dalLocalizer["limitReached"], HttpStatusCode.NotFound);

            querry.IsActive = update.IsActive;
            querry.ExpiredAt = update.Expired;
            querry.Discount = update.Discount;
            querry.UseLimit = update.UseLimit;

            _context.Set<Cupon>().Update(querry);

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }
    }
}