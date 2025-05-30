using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.BlogDtos;
using Entities.DTOs.BlogDtos.Admin;
using Entities.DTOs.BlogDtos.Update;
using Entities.DTOs.Helpers;
using Entities.Enums;

namespace DataAccess.Abstract
{
    public interface IBlogDAL : IService
    {
        Task<IDataResult<UpdateData>> BlogUpdateData(Guid blogId);
        Task<IResult> Update(UpdateData updateData, ReqFrom req);
        Task<IDataResult<string>> CategoryName(Guid id);
        Task<IDataResult<BaseDto<GetAll>>> GetAll(int page, int limit, ReqFrom req);
        Task<IDataResult<BaseDto<GetAllDetailed>>> GetAll(int page, int limit, Guid? categroyId, ReqFrom reqFrom);
        Task<IDataResult<object>> GetAll(Guid id, int page, int limit, ReqFrom req);
        Task<IResult> Create(Create create, Guid? userName, Superiority superiority);
        Task<IResult> DeleteCategory(Guid categoryId);
        Task<IResult> Delete(Guid id, ReqFrom req);
        Task<IDataResult<Get>> GetById(Guid id, ReqFrom req);
        Task<IResult> CreateCategory(List<CreateCategory> createCategory);
        Task<IDataResult<List<GetCategories>>> GetCategories();
        Task<IDataResult<GetComment>> AddComment(AddComment comment);
        Task<IDataResult<GetComment>> AddReplyComment(AddComment comment);
        Task<IDataResult<BaseDto<GetComment>>> CommentReply(Guid id, List<Guid> ids, int page, int limit);
        Task<IResult> RemoveComment(Guid id, string userName);
        Task<IDataResult<BaseDto<GetComment>>> GetComments(Guid id, List<Guid> ids, int page, int limit);
    }
}