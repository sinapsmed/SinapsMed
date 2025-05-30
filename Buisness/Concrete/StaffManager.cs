using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Staff;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.StaffDtos.Body;
using Entities.DTOs.StaffDtos.Return;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class StaffManager : IStaffService
    {
        private readonly StaffServiceFactory _staffServiceFactory;
        private readonly IStringLocalizer<CommonLocalizer> _commonLocalizer;

        public StaffManager(StaffServiceFactory staffServiceFactory, IStringLocalizer<CommonLocalizer> commonLocalizer)
        {
            _staffServiceFactory = staffServiceFactory;
            _commonLocalizer = commonLocalizer;
        }

        public async Task<IResult> AddAsync(StaffCreate staff)
        {
            try
            {
                var _dal = _staffServiceFactory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddAsync(staff);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<AllStaff>>> AllAsync(int page)
        {
            try
            {
                var _dal = _staffServiceFactory.GetService(ServiceFactoryType.Read);
                var response = await _dal.AllAsync(page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<AllStaff>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            try
            {
                var _dal = _staffServiceFactory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteAsync(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}