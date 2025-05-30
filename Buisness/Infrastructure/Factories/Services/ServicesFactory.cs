using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Services.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Services
{
    public class ServicesFactory
    {
        private readonly IServiceProvider _provider;

        public ServicesFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IServiceDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _provider.GetRequiredService<EFServiceCreateDAL>(),
                ServiceFactoryType.Read => _provider.GetRequiredService<EFServiceReadDAL>(),
                ServiceFactoryType.Update => _provider.GetRequiredService<EFServiceUpdateDAL>(),
                ServiceFactoryType.Delete => _provider.GetRequiredService<EFServiceDeleteDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}