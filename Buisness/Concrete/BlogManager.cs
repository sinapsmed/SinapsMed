using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Blogs;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.BlogDtos;
using Entities.DTOs.BlogDtos.Admin;
using Entities.DTOs.BlogDtos.Update;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class BlogManager : Manager, IBlogService
    {
        private readonly IStringLocalizer<CommonLocalizer> _localizer;
        private readonly BlogServiceFactory _factory;

        public BlogManager(IStringLocalizer<CommonLocalizer> localizer, BlogServiceFactory factory)
        {
            _localizer = localizer;
            _factory = factory;
        }

        public async Task<IDataResult<GetComment>> AddComment(AddComment comment)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddComment(comment);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<GetComment>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<GetComment>> AddReplyComment(AddComment comment)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddReplyComment(comment);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<GetComment>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<UpdateData>> BlogUpdateData(Guid blogId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.BlogUpdateData(blogId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UpdateData>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<string>> CategoryName(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.CategoryName(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetComment>>> CommentReply(Guid id, List<Guid> ids, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.CommentReply(id, ids, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetComment>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Create(Create create, Guid? userName, Superiority superiority)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.Create(create, userName, superiority);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> CreateCategory(List<CreateCategory> createCategory)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.CreateCategory(createCategory);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Delete(Guid id, ReqFrom req)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.Delete(id, req);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteCategory(categoryId);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetAll>>> GetAll(int page, int limit, ReqFrom req)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetAll(page, limit, req);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetAll>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<object>> GetAll(Guid id, int page, int limit, ReqFrom req)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetAll(id, page, limit, req);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<object>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetAllDetailed>>> GetAll(int page, int limit, Guid? categroyId, ReqFrom reqFrom)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetAll(page, limit, categroyId, reqFrom);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetAllDetailed>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<Get>> GetById(Guid id, ReqFrom req)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetById(id, req);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Get>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<GetCategories>>> GetCategories()
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetCategories();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetCategories>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetComment>>> GetComments(Guid id, List<Guid> ids, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.GetComments(id, ids, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetComment>>(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> RemoveComment(Guid id, string userName)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.RemoveComment(id, userName);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Update(UpdateData updateData, ReqFrom req)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Update);
                var response = await _dal.Update(updateData, req);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}