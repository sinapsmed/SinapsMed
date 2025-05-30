using System.Net;
using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.BlogEntities;
using Entities.DTOs.BlogDtos;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Blogs.CRUD
{
    public class EFBlogDeleteDAL : BlogAdapter
    {
        private readonly IRepositoryBase<Blog, GetAll, AppDbContext> _repo;
        private readonly IRepositoryBase<BlogCategory, GetAll, AppDbContext> _categoryRepo;

        public EFBlogDeleteDAL(
            AppDbContext context,
            IStringLocalizer<BlogAdapter> dalLocalizer,
            IRepositoryBase<Blog, GetAll, AppDbContext> repo,
            IRepositoryBase<BlogCategory, GetAll, AppDbContext> categoryRepo)
            : base(context, dalLocalizer)
        {
            _repo = repo;
            _categoryRepo = categoryRepo;
        }

        public override async Task<IResult> Delete(Guid id, ReqFrom req)
        {
            var blog = await _context.Blogs
                .Include(c => c.Comments)
                .Include(c => c.Languages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (req.Superiority is Superiority.Expert)
            {
                if (req.RequesterId != Guid.Empty)
                {
                    if (blog.ExpertId != req.RequesterId)
                    {
                        return new ErrorResult(_dalLocalizer["prohibited"]);
                    }
                }
                else
                {
                    return new ErrorResult(_dalLocalizer["prohibited"]);
                }
            }

            if (blog is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound, $"Blog Not found with {id} ID");

            return await _repo.Remove(blog, _context);
        }
        public override async Task<IResult> DeleteCategory(Guid categoryId)
        {
            var category = await _context.Set<BlogCategory>()
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            return await _categoryRepo.Remove(category, _context);
        }
        
        public override Task<IResult> RemoveComment(Guid id, string userName)
        {
            throw new NotImplementedException();
        }
    }
}