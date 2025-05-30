using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Emails;
using Entities.Concrete.Staff;
using Entities.DTOs.StaffDtos.Return;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.SQLServer.EFDALs.Staff.CRUD
{
    public class EFStaffDeleteDAL : StaffAdapter
    {
        public EFStaffDeleteDAL(AppDbContext context, IRepositoryBase<Support, AllStaff, AppDbContext> repostory) : base(context, repostory)
        {
        }

        public override async Task<IResult> DeleteAsync(Guid id)
        {
            var staff = await _context.Set<Support>()
                .Include(c => c.WorkSpaceEmail)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (staff == null)
                return new ErrorResult("Staff not found");

            var result = await _repostory.Remove(staff, _context);

            if (result.Success)
            {
                _context.Set<WorkSpaceEmail>()
                   .Remove(staff.WorkSpaceEmail);

                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return new ErrorResult("Staff could not be deleted");
            }
        }
    }
}