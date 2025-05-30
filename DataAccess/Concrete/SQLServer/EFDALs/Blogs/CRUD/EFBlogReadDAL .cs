using Core.Utilities.Results.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Abstract;
using Entities.Concrete.BlogEntities;
using Entities.Concrete.Experts;
using Entities.Concrete.UserEntities;
using Entities.DTOs.BlogDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Entities.DTOs.BlogDtos.Update;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Entities.DTOs.BlogDtos.Admin;

namespace DataAccess.Concrete.SQLServer.EFDALs.Blogs.CRUD
{
    public class EFBlogReadDAL : BlogAdapter
    {
        private readonly IRepositoryBase<Blog, GetAll, AppDbContext> _repo;
        private readonly IRepositoryBase<Blog, GetAllDetailed, AppDbContext> _repoDetailed;
        private readonly IRepositoryBase<Comment, GetComment, AppDbContext> _comment;
        private readonly IRepositoryBase<BlogCategory, GetCategories, AppDbContext> _blogCategories;
        private readonly IRepositoryBase<CommentReply, GetComment, AppDbContext> _commentReply;
        public EFBlogReadDAL(
            AppDbContext context,
            IStringLocalizer<BlogAdapter> dalLocalizer,
            IRepositoryBase<CommentReply, GetComment, AppDbContext> commentReply,
            IRepositoryBase<Blog, GetAll, AppDbContext> repo,
            IRepositoryBase<Comment, GetComment, AppDbContext> comment,
            IRepositoryBase<BlogCategory, GetCategories, AppDbContext> blogCategories,
            IRepositoryBase<Blog, GetAllDetailed, AppDbContext> repoDetailed)
            : base(context, dalLocalizer)
        {
            _commentReply = commentReply;
            _repo = repo;
            _comment = comment;
            _blogCategories = blogCategories;
            _repoDetailed = repoDetailed;
        }
        public override async Task<IDataResult<string>> CategoryName(Guid id)
        {
            using (var context = new AppDbContext())
            {
                var blogCategory = await context.BlogCategories.Include(c => c.Languages).FirstOrDefaultAsync(c => c.Id == id);
                if (blogCategory is null)
                    return new ErrorDataResult<string>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, "Id is invalid or crashed");

                var data = blogCategory.Languages.FirstOrDefault(c => c.Code == _cultre)?.Name;

                if (string.IsNullOrEmpty(data))
                    return new ErrorDataResult<string>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, "Id is invalid or crashed");

                return new SuccessDataResult<string>(data: data, HttpStatusCode.OK);
            }
        }
        public override async Task<IDataResult<BaseDto<GetComment>>> CommentReply(Guid id, List<Guid> ids, int page, int limit)
        {
            using (var context = new AppDbContext())
            {
                var querry = context.Set<CommentReply>()
                    .Where(c => c.ParentId == id && !ids.Any(x => x == c.Id))
                    .OrderByDescending(c => c.CreatedAt);

                var dtoFilter = new DtoFilter<CommentReply, GetComment>
                {
                    Limit = limit,
                    Page = page,
                    Selector = BlogSelector.CommentReplies()
                };

                return await _commentReply.GetAllAsync(querry, dtoFilter);
            }
        }
        public override async Task<IDataResult<BaseDto<GetAll>>> GetAll(int page, int limit, ReqFrom reqFrom)
        {
            var blogs = _context.Set<Blog>()
                .Include(c => c.Languages)
                .OrderByDescending(c => c.CreatedAt).AsQueryable();

            if (reqFrom.Superiority is Superiority.Expert)
            {
                blogs = blogs.Where(c => c.ExpertId == reqFrom.RequesterId);
            }

            var selector = BlogSelector.SelectAll(_cultre);

            DtoFilter<Blog, GetAll> dtoFilter = new DtoFilter<Blog, GetAll>
            {
                Page = page,
                Limit = limit,
                Selector = selector
            };

            return await _repo.GetAllAsync(blogs, dtoFilter);
        }
        public override async Task<IDataResult<Get>> GetById(Guid id, ReqFrom reqFrom)
        {
            var blog = await _context.Blogs
                .Include(c => c.Expert)
                .Include(c => c.Category)
                .ThenInclude(c => c.Languages)
                .Include(c => c.Languages)
                .Include(c => c.Comments)
                .ThenInclude(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == id);

            if ((blog is null) || (reqFrom.Superiority is Superiority.Expert && blog.ExpertId != reqFrom.RequesterId))
                return new ErrorDataResult<Get>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, $"Blog Not found with {id} ID");

            blog.View++;
            await _context.SaveChangesAsync();
            var data = BlogSelector.SelectSingle(blog, _cultre);

            return new SuccessDataResult<Get>(data, HttpStatusCode.OK);
        }
        public override async Task<IDataResult<object>> GetAll(Guid id, int page, int limit, ReqFrom reqFrom)
        {
            var category = await _context.Set<BlogCategory>().Include(c => c.Languages).FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                return new ErrorDataResult<object>(_dalLocalizer["categoryNotFound"], HttpStatusCode.NotFound, "We cannot find Category");

            var blogs = _context.Set<Blog>()
                .Include(c => c.Languages)
                .Where(c => c.CategoryId == category.Id)
                .AsQueryable();

            if (reqFrom.Superiority is Superiority.Expert)
            {
                blogs = blogs.Where(c => c.ExpertId == reqFrom.RequesterId);
            }

            var selector = BlogSelector.SelectAll(_cultre);

            DtoFilter<Blog, GetAll> dtoFilter = new DtoFilter<Blog, GetAll>
            {
                Page = page,
                Limit = limit,
                Selector = selector
            };
            var result = await _repo.GetAllAsync(blogs, dtoFilter);

            var data = new
            {
                Data = result.Data,
                CategoryName = category.Languages.FirstOrDefault(c => c.Code == _cultre).Name
            };

            return new SuccessDataResult<object>(data, HttpStatusCode.OK);
        }
        public override async Task<IDataResult<BaseDto<GetComment>>> GetComments(Guid id, List<Guid> ids, int page, int limit)
        {
            using (var context = new AppDbContext())
            {
                var querry = context.Set<Comment>()
                    .Include(c => c.Replies)
                    .OrderByDescending(c => c.CreatedAt)
                    .Where(c => c.BlogId == id && !ids.Any(x => x == c.Id));

                var selector = BlogSelector.Comments();

                var filter = new DtoFilter<Comment, GetComment>
                {
                    Limit = limit,
                    Page = page,
                    Selector = selector
                };

                return await _comment.GetAllAsync(querry, filter);
            }
        }
        public override async Task<IDataResult<List<GetCategories>>> GetCategories()
        {
            var categries = _context.Set<BlogCategory>();

            var selector = BlogSelector.Category(_cultre);

            return await _blogCategories.GetAllAsync(categries, selector);
        }
        public override async Task<IDataResult<UpdateData>> BlogUpdateData(Guid blogId)
        {
            var blog = await _context.Blogs
                .Include(c => c.Languages)
                .FirstOrDefaultAsync(c => c.Id == blogId);

            if (blog is null)
                return new ErrorDataResult<UpdateData>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, $"Blog Not found with {blogId} ID");

            var data = BlogSelector.UpdateData(blog);

            return new SuccessDataResult<UpdateData>(data, HttpStatusCode.OK);
        }
        public override async Task<IDataResult<BaseDto<GetAllDetailed>>> GetAll(int page, int limit, Guid? categroyId, ReqFrom reqFrom)
        {
            var blogs = _context.Set<Blog>()
                .Include(C => C.Expert)
                .Include(c => c.Languages)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            var selector = BlogSelector.SelectAllDetailed(_cultre);

            if (reqFrom.Superiority is Superiority.Expert)
            {
                blogs = blogs.Where(c => c.ExpertId == reqFrom.RequesterId);
            }

            if (categroyId is not null)
                blogs = blogs.Where(c => c.CategoryId == categroyId);

            DtoFilter<Blog, GetAllDetailed> dtoFilter = new DtoFilter<Blog, GetAllDetailed>
            {
                Page = page,
                Limit = limit,
                Selector = selector
            };

            return await _repoDetailed.GetAllAsync(blogs, dtoFilter);
        }
    }
}