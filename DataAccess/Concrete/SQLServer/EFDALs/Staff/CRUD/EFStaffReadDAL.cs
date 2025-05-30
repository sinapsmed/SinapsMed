using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.OrderEntities;
using Entities.Concrete.Staff;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.StaffDtos.Return;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.SQLServer.EFDALs.Staff.CRUD
{
    public class EFStaffReadDAL : StaffAdapter
    {
        private readonly IRepositoryBase<OrderItem, StaffAnalyses, AppDbContext> _orderItemRepository;
        public EFStaffReadDAL(AppDbContext context, IRepositoryBase<Support, AllStaff, AppDbContext> repostory, IRepositoryBase<OrderItem, StaffAnalyses, AppDbContext> orderItemRepository) : base(context, repostory)
        {
            _orderItemRepository = orderItemRepository;
        }
        public override async Task<IDataResult<BaseDto<StaffAnalyses>>> Analyses(string? userId, int page)
        {
            var querry = _context
                .Set<OrderItem>()
                .Include(c => c.Order)
                    .ThenInclude(c => c.User)
                .Where(c => c.Type == ItemType.Analysis)
                .Include(c => c.Analysis)
                        .ThenInclude(c => c.Category).AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                querry = querry.Where(c => c.Order.UserId == userId);

            var filter = new DtoFilter<OrderItem, StaffAnalyses>
            {
                Limit = 10,
                Page = page,
                Selector = StaffSelector.Analyses()
            };

            return await _orderItemRepository.GetAllAsync(querry, filter);
        }

        public override async Task<IDataResult<BaseDto<AllStaff>>> AllAsync(int page = 1)
        {
            var querry = _context.Set<Support>()
                .Include(c => c.WorkSpaceEmail)
                .AsQueryable();

            var filter = new DtoFilter<Support, AllStaff>
            {
                Limit = 10,
                Page = page,
                Selector = StaffSelector.All()
            };

            return await _repostory.GetAllAsync(querry, filter);
        }
    }
}