using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Analyses;
using Entities.Concrete.BasketEntities;
using Entities.DTOs.BasketDtos.BodyDtos;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD
{
    public class EFBasketCreateDAL : BasketAdapter
    {
        public EFBasketCreateDAL(
            AppDbContext context,
            IStringLocalizer<BasketAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }

        public override async Task<IResult> AddItem(AddItem item)
        {
            if (item.AppointmentId == null && item.AnalysisId == null)
            {
                return new ErrorResult(_dalLocalizer["shouldOneServiceAdded"], HttpStatusCode.BadRequest);
            }
            var user = await _context.Users
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                        .ThenInclude(c => c.Analysis)
                .Include(c => c.Basket)
                    .ThenInclude(c => c.Items)
                        .ThenInclude(c => c.Appointment)
                .FirstOrDefaultAsync(c => c.Id == item.UserId);

            if (user is null)
            {
                return new ErrorResult(_dalLocalizer["userNotFound"], HttpStatusCode.NotFound);
            }

            if (user.Basket is null)
            {
                user.Basket = new();
            }

            var addedItem = item.Map<BasketItem, AddItem>();

            if (item.Type is ItemType.Analysis)
            {
                var analysis = await _context.Set<Analysis>()
                    .FirstOrDefaultAsync(c => c.Id == item.AnalysisId);

                if (analysis is null)
                {
                    return new ErrorResult(_dalLocalizer["analysisNotFound"], HttpStatusCode.NotFound);
                }

                addedItem.ClinicId = analysis.ClinicId;
            }

            var basketItem = user.Basket.Items.FirstOrDefault(c =>

            (c.AnalysisId == item.AnalysisId && item.AnalysisId != null)
                ||
            (c.AppointmentId == item.AppointmentId && item.AppointmentId != null));

            if (basketItem is null)
            {
                user.Basket.Items.Add(addedItem);
            }

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }

    }
}