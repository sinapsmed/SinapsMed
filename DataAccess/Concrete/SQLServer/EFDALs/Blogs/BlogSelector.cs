using System.Linq.Expressions;
using Entities.Concrete.BlogEntities;
using Entities.DTOs.BlogDtos;
using Entities.DTOs.BlogDtos.Admin;
using Entities.DTOs.BlogDtos.Update;
using Entities.Enums.Blog;

namespace DataAccess.Concrete.SQLServer.EFDALs.Blogs
{
    public static class BlogSelector
    {
        public static Expression<Func<CommentReply, GetComment>> CommentReplies()
        {
            return c => new GetComment
            {
                CreatedAt = c.CreatedAt,
                Description = $"<span style = \"color:#1D4ED8;\">{c.Parent.User.FullName} </span><span> : {c.Description}</span>",
                Id = c.Id,
                UserName = c.User.FullName,
                UserImage = c.User.ImageUrl,
            };
        }

        public static UpdateData UpdateData(Blog blog)
        {
            return new UpdateData
            {
                BlogId = blog.Id,
                ImageUrl = blog.ImageUrl,
                Languages = blog.Languages.Select(c => new UpdateLang
                {
                    Code = c.Code,
                    Description = c.Description,
                    Preview = c.Preview,
                    Title = c.Title
                }).ToList()
            };
        }
        public static Expression<Func<Comment, GetComment>> Comments()
        {
            return c => new GetComment
            {
                CreatedAt = c.CreatedAt,
                UserName = c.User.FullName,
                Description = c.Description,
                ReplyCount = c.Replies.Count,
                Id = c.Id,
                UserImage = c.User.ImageUrl,
            };
        }
        public static Expression<Func<Blog, GetAllDetailed>> SelectAllDetailed(string cultre)
        {
            return c => new GetAllDetailed
            {
                Title = c.Languages.FirstOrDefault(c => c.Code == cultre).Title,
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.Creator == Creator.Expert ? $"{c.Expert.Specality} {c.Expert.FullName}" : "Sinapsmed.com"
            };
        }
        public static Expression<Func<Blog, GetAll>> SelectAll(string cultre)
        {
            return c => new GetAll
            {
                Title = c.Languages.FirstOrDefault(c => c.Code == cultre).Title,
                Id = c.Id,
                Href = c.Languages.FirstOrDefault(c => c.Code == cultre).SeoUrl,
                Preview = c.Languages.FirstOrDefault(c => c.Code == cultre).Preview,
                ImgUrl = c.ImageUrl
            };
        }
        public static Expression<Func<BlogCategory, GetCategories>> Category(string cultre)
        {
            return c => new GetCategories
            {
                Id = c.Id,
                Href = $"/blogs/{c.Languages.FirstOrDefault(c => c.Code == cultre).Href}?catId={c.Id}",
                Name = c.Languages.FirstOrDefault(c => c.Code == cultre).Name
            };
        }

        public static Get SelectSingle(Blog blog, string _cultre)
        {
            return new Get
            {
                Id = blog.Id,
                CategoryId = blog.Category.Id,
                CategoryName = blog.Category.Languages.FirstOrDefault(c => c.Code == _cultre).Name,
                Href = blog.Languages.FirstOrDefault(c => c.Code == _cultre).SeoUrl,
                CreatedAt = blog.CreatedAt,
                Creator = blog.Creator == Creator.Expert ? $"{blog.Expert.Specality} {blog.Expert.FullName}" : "Sinapsmed.com",
                CreatorImage = blog.Creator == Creator.Expert ? blog.Expert.PhotoUrl : null,
                CreateorRole = blog.Creator.ToString(),
                Description = blog.Languages.FirstOrDefault(c => c.Code == _cultre).Description,
                ImageUrl = blog.ImageUrl,
                Like = blog.Like,
                Name = blog.Languages.FirstOrDefault(c => c.Code == _cultre).Title,
                View = blog.View,
                CommentCount = blog.Comments.Sum(c => c.Replies.Count()) + blog.Comments.Count(),
                Preview = blog.Languages.FirstOrDefault(c => c.Code == _cultre).Preview
            };
        }
    }
}