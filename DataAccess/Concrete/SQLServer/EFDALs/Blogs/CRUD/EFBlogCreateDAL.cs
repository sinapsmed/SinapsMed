using DataAccess.Concrete.SQLServer.DataBase;
using System.Net;
using Core.DataAccess;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Entities.Concrete.BlogEntities;
using Entities.Concrete.UserEntities;
using Entities.DTOs.BlogDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Entities.Concrete.Experts;
using Entities.Enums.Blog;
using Entities.Enums;

namespace DataAccess.Concrete.SQLServer.EFDALs.Blogs.CRUD
{
    public class EFBlogCreateDAL : BlogAdapter
    {
        private readonly IRepositoryBase<Blog, GetAll, AppDbContext> _repo;
        private readonly IRepositoryBase<BlogCategory, GetCategories, AppDbContext> _blogCategories;
        private readonly IRepositoryBase<Comment, GetComment, AppDbContext> _comment;
        private readonly IRepositoryBase<CommentReply, GetComment, AppDbContext> _commentReply;
        private readonly UserManager<AppUser> _userManager;
        public EFBlogCreateDAL(
            AppDbContext context,
            IStringLocalizer<BlogAdapter> dalLocalizer,
            UserManager<AppUser> userManager,
            IRepositoryBase<Comment, GetComment, AppDbContext> comment,
            IRepositoryBase<CommentReply, GetComment, AppDbContext> commentReply,
            IRepositoryBase<Blog, GetAll, AppDbContext> repo,
            IRepositoryBase<BlogCategory, GetCategories, AppDbContext> blogCategories)
             : base(context, dalLocalizer)
        {
            _userManager = userManager;
            _comment = comment;
            _commentReply = commentReply;
            _repo = repo;
            _blogCategories = blogCategories;
        }

        public override async Task<IDataResult<GetComment>> AddComment(AddComment comment)
        {
            var user = await _userManager.FindByNameAsync(comment.UserName);
            if (user is null)
                return new ErrorDataResult<GetComment>(_dalLocalizer["unAuth"], HttpStatusCode.Unauthorized, $"We can not find user with Username is : {comment.UserName}");

            var blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == comment.BlogId);
            if (blog is null)
                return new ErrorDataResult<GetComment>(_dalLocalizer["blogNotFound"], HttpStatusCode.NotFound, $"We can not find Blog");

            var model = new Comment
            {
                BlogId = blog.Id,
                CreatedBy = user.UserName,
                Description = comment.Description,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                Replies = new List<CommentReply>()
            };

            var response = await _comment.AddAsync(model, _context);

            GetComment getComment = new GetComment
            {
                Id = model.Id,
                CreatedAt = model.CreatedAt,
                Description = model.Description,
                ReplyCount = model.Replies.Count,
                UserName = user.FullName,
                UserImage = user.ImageUrl
            };

            if (response.Success)
                return new SuccessDataResult<GetComment>(getComment, HttpStatusCode.OK);
            else
                return new ErrorDataResult<GetComment>(response.Message, response.StatusCode, "Problem occured");
        }
        public override async Task<IDataResult<GetComment>> AddReplyComment(AddComment comment)
        {
            using (var context = new AppDbContext())
            {
                var user = await _userManager.FindByNameAsync(comment.UserName);
                if (user is null)
                    return new ErrorDataResult<GetComment>(_dalLocalizer["unAuth"], HttpStatusCode.Unauthorized, $"We can not find user with Username is : {comment.UserName}");

                var parent = await context.Comments.FirstOrDefaultAsync(c => c.Id == comment.ParentId);
                if (parent is null)
                    return new ErrorDataResult<GetComment>(_dalLocalizer["commentNotFound"], HttpStatusCode.NotFound, $"We can not find Parent Comment");

                var model = new CommentReply
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Description = comment.Description,
                    CreatedBy = user.FullName,
                    UserId = user.Id,
                    ParentId = parent.Id,
                };

                var response = await _commentReply.AddAsync(model, context);

                GetComment getComment = new GetComment
                {
                    CreatedAt = model.CreatedAt,
                    Description = model.Description,
                    UserName = user.FullName,
                    UserImage = user.ImageUrl,
                    Id = model.Id
                };

                if (response.Success)
                    return new SuccessDataResult<GetComment>(getComment, HttpStatusCode.OK);
                else
                    return new ErrorDataResult<GetComment>(response.Message, response.StatusCode, "Problem occured");
            }
        }
        public override async Task<IResult> CreateCategory(List<CreateCategory> createCategory)
        {
            BlogCategory blogCategory = new BlogCategory
            {
                Blogs = new List<Blog>(),
                Languages = createCategory.Select(c => new BlogCategoryLang
                {
                    Code = c.Code,
                    Href = SeoHelper.ConverToSeo(c.Name, _cultre),
                    Name = c.Name
                }).ToList()
            };

            using (var context = new AppDbContext())
            {
                return await _blogCategories.AddAsync(blogCategory, context);
            }
        }
        public override async Task<IResult> Create(Create create, Guid? userName, Superiority superiority)
        {
            Blog blog = new Blog
            {
                CategoryId = create.CategoryId,
                Comments = new List<Comment>(),
                Languages = create.Languages.Select(c => new BlogLang
                {
                    Code = c.Code,
                    Description = c.Description,
                    SeoUrl = SeoHelper.ConverToSeo(c.Title, _cultre),
                    Title = c.Title,
                    Preview = c.Preview
                }).ToList(),
                ImageUrl = create.ImageUrl,
                Like = 0,
            };

            Expert expert = null;

            if (superiority is Superiority.Expert)
            {
                expert = await _context.Experts.FirstOrDefaultAsync(c => c.Id == userName);
                if (expert is null)
                {
                    return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.Unauthorized, "Expert token is invalid or crashed");
                }
                else
                {
                    blog.CreatedBy = expert.FullName;
                    blog.ExpertId = expert.Id;
                    blog.Creator = Creator.Expert;
                }
            }
            else
            {
                blog.Creator = Creator.Admin;
            }

            return await _repo.AddAsync(blog, _context);
        }
    }
}