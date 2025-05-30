using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Helpers;
using Entities.DTOs.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Custom
{
    public class EFCustomDAL(IEmailService _email,
            IStringLocalizer<EFCustomDAL> _dalLocalizer,
            IRepositoryBase<Offer, Offer, AppDbContext> _repo)
                : Manager, ICustomDAL
    {

        public async Task<IResult> Contact(CreateOffer offer)
        {
            Offer model = offer.Map<Offer, CreateOffer>();

            using (var context = new AppDbContext())
            {
                var result = await _repo.AddAsync(model, context, _dalLocalizer["succesSend"]);
                return result;
            }
        }

        public async Task<IResult> Delete(Guid id)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Set<Offer>().OrderBy(c => c.IsRead)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (entity is null)
                    return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound, $"We can not find Offer with id {id}");

                return await _repo.Remove(entity, context);
            }
        }

        public async Task<IDataResult<BaseDto<Offer>>> GetAll(int page, int limit)
        {
            using (var context = new AppDbContext())
            {
                var entites = context.Set<Offer>();

                DtoFilter<Offer, Offer> limitter = new DtoFilter<Offer, Offer>
                {
                    Limit = limit,
                    Page = page,
                    Selector = c => c
                };

                return await _repo.GetAllAsync(entites, limitter);
            }
        }

        public async Task<IResult> MarkAsRead(Guid id)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Set<Offer>().FirstOrDefaultAsync(c => c.Id == id);
                if (entity is null)
                    return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound, $"We can not find Offer with id {id}");

                entity.IsRead = true;

                return await _repo.Update(entity, context);
            }
        }

        public async Task<IResult> Reply(Guid id, string message)
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Set<Offer>().FirstOrDefaultAsync(c => c.Id == id);
                if (entity is null)
                    return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound, $"We can not find Offer with id {id}");
                var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "SupportAnswer.html");

                string fileContent = File.ReadAllText(url);

                fileContent = fileContent
                    .Replace("{{question}}", entity.Description)
                    .Replace("{{answer}}", message);

                await _email.SendEmailAsync(entity.Email, "Sualınız Cavablandırıldı", fileContent, "support");

                entity.SupportAnswer = message;
                entity.IsRead = true;

                await context.SaveChangesAsync();

                return new SuccessResult();
            }
        }
    }
}