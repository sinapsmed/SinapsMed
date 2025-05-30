
using Core.DataAccess;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD;
using Entities.Concrete.Emails;
using Entities.Concrete.Staff;
using Entities.DTOs.StaffDtos.Body;
using Entities.DTOs.StaffDtos.Return;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.SQLServer.EFDALs.Staff.CRUD
{
    public class EFStaffCreateDAL : StaffAdapter
    {
        private readonly EFWorkSpaceEmailCreateDAL _emailCreateDAL;
        private readonly IConfiguration _config;
        public EFStaffCreateDAL(AppDbContext context, IRepositoryBase<Support, AllStaff, AppDbContext> repostory, EFWorkSpaceEmailCreateDAL emailCreateDAL, IConfiguration config) : base(context, repostory)
        {
            _emailCreateDAL = emailCreateDAL;
            _config = config;
        }

        public override async Task<IResult> AddAsync(StaffCreate staff)
        {
            var changedEmail = staff.FullName.Replace(" ", "").ToLower().ConverToSeo("az");

            var newAddress = $"{changedEmail}@{_config["Email:Domain"]}";

            var emailExists = await _context.Set<WorkSpaceEmail>().AnyAsync(c => c.Email == newAddress);
            
            if (emailExists)
                return new ErrorResult("Email already exists,please try another name or add a number to the name");

            var email = await _emailCreateDAL.AddAsync(newAddress, staff.Password, staff.FullName);
            if (!email.Success)
                return email;

            var workSpaceEmailId = await _context.Set<WorkSpaceEmail>().FirstOrDefaultAsync(c => c.Email == newAddress);
            if (workSpaceEmailId == null)
                return new ErrorResult("Email not found");

            var newStaff = new Support
            {
                FullName = staff.FullName,
                WorkSpaceEmailId = workSpaceEmailId.Id,
            };

            var result = await _repostory.AddAsync(newStaff, _context);
            return result;


        }
    }
}