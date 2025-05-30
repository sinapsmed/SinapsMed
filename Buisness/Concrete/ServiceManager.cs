using System.Net;
using Buisness.Abstract;
using Buisness.Extentions;
using Buisness.Infrastructure.Factories.Services;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using Entities.DTOs.Helpers;
using Entities.DTOs.ServiceDtos.Create;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.ServiceDtos.Update;
using Entities.DTOs.SpecalitiyDtos.Create;
using Entities.DTOs.SpecalitiyDtos.Get;
using Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class ServiceManager : IServiceService
    {
        private readonly IStringLocalizer<CommonLocalizer> _commonLocalizer;
        private readonly IWebHostEnvironment _env;
        private readonly ServicesFactory _factory;

        public ServiceManager(IStringLocalizer<CommonLocalizer> commonLocalizer, IWebHostEnvironment env, ServicesFactory factory)
        {
            _commonLocalizer = commonLocalizer;
            _env = env;
            _factory = factory;
        }

        public async Task<IResult> AddPeriod(PeriodDto period)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddPeriod(period);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddService(CreateSpecailty createSpecailty)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddService(createSpecailty);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> CreateCategory(List<CreateCat> cats)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.CreateCategory(cats);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<GetCat>>> GetCategories()
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetCategories();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetCat>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<GetHeader>>> GetHeaders()
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetHeaders();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetHeader>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<GetService>>> GetServices(Guid? id, Guid? expertId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetServices(id, expertId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetService>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<PeriodGetDto>>> Periods(Guid specId, ReqFrom from)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.Periods(specId, from);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<PeriodGetDto>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Detail>> ServiceDetail(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.ServiceDetail(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Detail>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<GetSpecality>>> AllServices(int page, Guid? expertId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.AllServices(page, expertId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetSpecality>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<ServiceUpdateGet>> UpdateServiceData(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.UpdateServiceData(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ServiceUpdateGet>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<ServicePeriodUpdateGet>> UpdateServicePeriodData(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.UpdateServicePeriodData(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<ServicePeriodUpdateGet>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Delete(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.Delete(id);
                if (response.Success)
                {
                    string[] splits = response.Data.Split("/");
                    FileExtention.DeleteFile(splits[^1], _env, "assets", "Files", splits[^2]);
                }
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeletePeriod(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeletePeriod(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(ServiceUpdateGet updateGet)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.Update(updateGet);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdatePeriod(ServicePeriodUpdateGet updateGet)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.UpdatePeriod(updateGet);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<CategoryUpdateGet>> UpdateCategoryData(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.UpdateCategoryData(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<CategoryUpdateGet>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdateCategory(CategoryUpdateGet updateGet)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.UpdateCategory(updateGet);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteCategory(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteCategory(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddComplaint(CreateComplaint complaint)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddComplaint(complaint);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<IEnumerable<GetComplaints>>> GetComplaints(Guid serviceId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetComplaints(serviceId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<IEnumerable<GetComplaints>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteComplaint(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteComplaint(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}