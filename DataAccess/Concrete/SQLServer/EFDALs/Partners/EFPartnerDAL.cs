using System.Linq.Expressions;
using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Partners;
using Entities.DTOs.PartnerDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Partners
{
    public class EFPartnerDAL : Manager, IPartnerDAL
    {
        private readonly AppDbContext _context;
        private readonly IRepositoryBase<Partner, Get, AppDbContext> _repo;
        private readonly IStringLocalizer<EFPartnerDAL> _dalLoclizer;
        public EFPartnerDAL(
            AppDbContext context,
             IRepositoryBase<Partner, Get, AppDbContext> repo,
              IStringLocalizer<EFPartnerDAL> dalLoclizer)
        {
            _context = context;
            _repo = repo;
            _dalLoclizer = dalLoclizer;
        }

        public async Task<IResult> Create(Create create)
        {
            Partner partner = new Partner
            {
                LogoUrl = create.PhotoUrl,
                Name = create.Name,
            };
            return await _repo.AddAsync(partner, _context);
        }

        public async Task<IResult> Delete(Guid id)
        {
            Partner partner = await _context.Partners.FirstOrDefaultAsync(c => c.Id == id);
            if (partner is null)
                return new ErrorResult(_dalLoclizer["notFound"], HttpStatusCode.NotFound, $"Partner Is not FOund id {id}");
            return await _repo.Remove(partner, _context);
        }

        public async Task<IDataResult<List<Get>>> GetAll()
        {

            IQueryable<Partner> partners = _context.Set<Partner>();

            var selector = Selector(partners);

            return await _repo.GetAllAsync(partners, selector);
        }

        public async Task<IResult> Update(Update update)
        {
            Partner partner = await _context.Partners.FirstOrDefaultAsync(c => c.Id == update.Id);

            partner.Name = update.Name ?? partner.Name;
            partner.LogoUrl = update.LogoUrl ?? partner.LogoUrl;

            if (partner is null)
                return new ErrorResult(_dalLoclizer["notFound"], HttpStatusCode.NotFound, $"Partner Is not Found id {update.Id}");

            return await _repo.Update(partner, _context);
        }

        #region Private Methods
        private Expression<Func<Partner, Get>> Selector(IQueryable<Partner> partners)
        {
            return c => new Get
            {
                LogoUrl = c.LogoUrl,
                Name = c.Name,
                Id = c.Id,
                Number = c.Number,
            };
        }
        #endregion 
    }
}