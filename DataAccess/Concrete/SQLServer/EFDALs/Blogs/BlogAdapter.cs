using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.BlogDtos;
using Entities.DTOs.BlogDtos.Admin;
using Entities.DTOs.BlogDtos.Update;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Blogs
{
    public class BlogAdapter : BaseAdapter, IBlogDAL
    {
        protected IStringLocalizer<BlogAdapter> _dalLocalizer;
        public BlogAdapter(AppDbContext context, IStringLocalizer<BlogAdapter> dalLocalizer) : base(context)
        {
            _dalLocalizer = dalLocalizer;
        }

        public virtual Task<IDataResult<GetComment>> AddComment(AddComment comment)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<GetComment>> AddReplyComment(AddComment comment)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<UpdateData>> BlogUpdateData(Guid blogId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<string>> CategoryName(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<GetComment>>> CommentReply(Guid id, List<Guid> ids, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Create(Create create, Guid? userName, Superiority superiority)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> CreateCategory(List<CreateCategory> createCategory)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Delete(Guid id, ReqFrom req)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<GetAll>>> GetAll(int page, int limit, ReqFrom req)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<object>> GetAll(Guid id, int page, int limit, ReqFrom req)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<GetAllDetailed>>> GetAll(int page, int limit, Guid? categroyId, ReqFrom reqFrom)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<Get>> GetById(Guid id, ReqFrom req)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<GetCategories>>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<GetComment>>> GetComments(Guid id, List<Guid> ids, int page, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> RemoveComment(Guid id, string userName)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Update(UpdateData updateData, ReqFrom req)
        {
            throw new NotImplementedException();
        }
    }
}