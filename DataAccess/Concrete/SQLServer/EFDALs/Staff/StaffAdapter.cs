using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Staff;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.StaffDtos.Body;
using Entities.DTOs.StaffDtos.Return;

namespace DataAccess.Concrete.SQLServer.EFDALs.Staff
{
    public class StaffAdapter : BaseAdapter, IStaffDAL
    {
        protected readonly IRepositoryBase<Support, AllStaff, AppDbContext> _repostory;
        public StaffAdapter(AppDbContext context, IRepositoryBase<Support, AllStaff, AppDbContext> repostory) : base(context)
        {
            _repostory = repostory;
        }

        public virtual Task<IResult> AddAsync(StaffCreate staff)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<AllStaff>>> AllAsync(int page = 1)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<StaffAnalyses>>> Analyses(string userId,int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<AppointmentList>>> Appointments(int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<AppointmentStaffDto>> AppointmentDetail()
        {
            throw new NotImplementedException();
        }
    }
}