using System.Net;
using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.CuponCodes;
using Entities.DTOs.CuponDtos;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Cupons.CRUD
{
    public class EFCuponCreateDAL : EFCuponCodeAdapter
    {
        private readonly IRepositoryBase<Cupon, Create, AppDbContext> _cupon;

        public EFCuponCreateDAL(IRepositoryBase<Cupon, Create, AppDbContext> cupon, AppDbContext context, IStringLocalizer<EFCuponCodeAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
            _cupon = cupon;
        }

        public override async Task<IResult> Create(Create create)
        {
            if (await _context.Cupons.AnyAsync(c => c.Code.ToLower() == create.Code.Trim().ToLower()))
                return new ErrorResult(_dalLocalizer["alreadyExsist", create.Code], HttpStatusCode.BadRequest);

            Cupon cupon = new Cupon()
            {
                Code = create.Code.Trim(),
                Discount = create.Discount,
                ExpiredAt = DateTime.SpecifyKind(create.Expired, DateTimeKind.Utc),
                StartAt = DateTime.SpecifyKind(create.Start, DateTimeKind.Utc),
                Type = create.Type,
                UseLimit = create.UseLimit,
                UseLimitForPerUser = create.UseLimitForPerUser,
                IsActive = false
            };

            if (create.SpesficUserIds.Count() > 0)
            {
                foreach (var userId in create.SpesficUserIds)
                {
                    cupon.SpesficCuponUsers.Add(new SpesficCuponUser
                    {
                        UserId = userId
                    });
                }
            }

            if (create.SpesficServiceIds.Count() > 0 && create.Type != CuponType.Common)
            {
                foreach (var serviceId in create.SpesficServiceIds)
                {
                    cupon.SpesficServiceCupons.Add(new SpesficServiceCupon
                    {
                        ServiceId = serviceId
                    });
                }
            }

            return await _cupon.AddAsync(cupon, _context);
        }

    }
}