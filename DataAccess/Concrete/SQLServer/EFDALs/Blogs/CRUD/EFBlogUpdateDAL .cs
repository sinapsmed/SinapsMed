using System.Net;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.BlogEntities;
using Entities.DTOs.BlogDtos.Update;
using Entities.DTOs.Helpers;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Blogs.CRUD
{
    public class EFBlogUpdateDAL : BlogAdapter
    {

        public EFBlogUpdateDAL(AppDbContext context, IStringLocalizer<BlogAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }

        public override async Task<IResult> Update(UpdateData updateData, ReqFrom reqFrom)
        {
            var blog = await _context.Blogs
                .Include(c => c.Languages)
                .FirstOrDefaultAsync(c => c.Id == updateData.BlogId);

            if (blog is null)
                return new ErrorDataResult<UpdateData>(_dalLocalizer["notFound"], HttpStatusCode.NotFound, $"Blog Not found with {updateData.BlogId} ID");

            if (reqFrom.Superiority is not Superiority.Admin)
            {
                if (reqFrom.Superiority is Superiority.Expert && blog.ExpertId != Guid.Empty)
                {
                    if (blog.ExpertId != updateData.Updater)
                    {
                        return new ErrorResult(_dalLocalizer["onlyExpertCanUpdate"]);
                    }
                }
                else
                {
                    return new ErrorResult(_dalLocalizer["onlyExpertCanUpdate"]);
                }
            }

            blog.ImageUrl = updateData.ImageUrl ?? blog.ImageUrl;

            foreach (var language in updateData.Languages)
            {
                var lang = blog.Languages.FirstOrDefault(c => c.Code == language.Code);
                if (lang is null)
                {
                    blog.Languages.Add(new BlogLang
                    {
                        Code = language.Code,
                        Description = language.Description,
                        Title = language.Title,
                        Preview = language.Preview,
                        SeoUrl = SeoHelper.ConverToSeo(language.Title, language.Code)
                    });
                }
                else
                {
                    if (language.Description != lang.Description)
                    {
                        lang.Description = language.Description;
                    }

                    if (language.Preview != lang.Preview)
                    {
                        lang.Preview = language.Preview;
                    }

                    if (language.Title != lang.Title)
                    {
                        lang.Title = language.Title;
                        lang.SeoUrl = SeoHelper.ConverToSeo(language.Title, language.Code);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return new SuccessResult(HttpStatusCode.OK);
        }
    }
}