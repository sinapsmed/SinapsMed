using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Blogs.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Blogs
{
    public class BlogServiceFactory
    {
        private readonly IServiceProvider _provider;

        public BlogServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IBlogDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _provider.GetRequiredService<EFBlogCreateDAL>(),
                ServiceFactoryType.Read => _provider.GetRequiredService<EFBlogReadDAL>(),
                ServiceFactoryType.Update => _provider.GetRequiredService<EFBlogUpdateDAL>(),
                ServiceFactoryType.Delete => _provider.GetRequiredService<EFBlogDeleteDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}