using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Analyses;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Analysis.Admin;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.DTOs.AnalysisDtos.Category.Admin;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class AnalysisManager : Manager, IAnalysisService
    {
        private readonly IStringLocalizer<CommonLocalizer> _common;
        private readonly AnalysesServiceFactory _factory;

        public AnalysisManager(IStringLocalizer<CommonLocalizer> common, AnalysesServiceFactory factory)
        {
            _common = common;
            _factory = factory;
        }

        public async Task<IResult> Add(Create create)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.Add(create);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddCat(CreateCategory createCategory)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddCat(createCategory);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddList(Microsoft.AspNetCore.Http.IFormFile file, string agentMail)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddList(file, agentMail);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Delete(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.Delete(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteCat(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteCat(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Get>>> GetAll(Filter filter)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetAll(filter);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Get>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<DetailedGet>>> GetAllDetailed(Filter filter)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetAllDetailed(filter);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<DetailedGet>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<GetCats>>> GetCats()
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetCats();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetCats>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<GetDetailedCats>>> GetDetailedCats()
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetDetailedCats();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetDetailedCats>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(Update update)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.Update(update);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> UpdateCat(UpdateCategory updateCategory)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.UpdateCat(updateCategory);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}