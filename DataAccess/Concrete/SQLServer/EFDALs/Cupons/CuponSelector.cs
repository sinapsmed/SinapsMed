using System.Linq.Expressions;
using Entities.Concrete.CuponCodes;
using Entities.Concrete.Services;
using Entities.DTOs.CuponDtos;

namespace DataAccess.Concrete.SQLServer.EFDALs.Cupons
{
    public static class CuponSelector
    {
        public static Expression<Func<Service, CuponService>> GetServices(string cultre)
        {
            return c => new CuponService
            {
                Id = c.Id.ToString(),
                Label = c.Languages.FirstOrDefault(c => c.Code == cultre).Title
            };
        }


        public static Expression<Func<CuponUsing, UsedCupon>> UsedCupons()
        {
            return c => new UsedCupon
            {
                Amount = c.Amount,
                Discount = c.Cupon.Discount,
                FullName = c.User.FullName,
                ImageUrl = c.User.ImageUrl,
                UnikalKey = c.User.UnicalKey,
                UsedAt = c.UsedAt
            };
        }
        public static Expression<Func<Cupon, Get>> GetCupon()
        {
            DateTime now = DateTime.UtcNow;
            return c => new Get
            {
                Code = c.Code,
                Id = c.Id,
                Type = c.Type,
                Used = c.UsedCupons.Count(),
                UseLimit = c.UseLimit,
                IsExpired = now > c.ExpiredAt,
                Discount = c.Discount
            };
        }

        public static Detail GetDetail(Cupon c)
        {
            DateTime now = DateTime.UtcNow;

            Detail detail = new()
            {
                Code = c.Code,
                Id = c.Id,
                Type = c.Type,
                Used = c.UsedCupons.Count(),
                UseLimit = c.UseLimit,
                IsExpired = now > c.ExpiredAt,
                Discount = c.Discount,
                ExpiredAt = c.ExpiredAt,
                StartAt = c.StartAt,
                UseLimitForPerUser = c.UseLimitForPerUser,
                CuponUsers = c.SpesficCuponUsers
                    .Select(x => new CuponUser
                    {
                        FullName = x.User.FullName,
                        ImageUrl = x.User.ImageUrl,
                        UnikalKey = x.User.UnicalKey,
                        UserId = x.UserId
                    }).ToList(),
                UsedCupons = c.UsedCupons
                    .Select(x => new UsedCupon
                    {
                        Amount = x.Amount,
                        Discount = c.Discount,
                        FullName = x.User.FullName,
                        ImageUrl = x.User.ImageUrl,
                        UnikalKey = x.User.UnicalKey,
                        UsedAt = x.UsedAt
                    }).ToList()
            };
            return detail;
        }

        public static Update UpdateData(Cupon c)
        {
            return new Update
            {
                Discount = c.Discount,
                Expired = c.ExpiredAt,
                Id = c.Id,
                IsActive = c.IsActive,
                Start = c.StartAt,
                UseableCount = c.UseLimit - c.UsedCupons.Count(),
                UseLimit = c.UseLimit
            };
        }
    }
}