using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Locations.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Locations
{
    public class LocationServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public LocationServiceFactory(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public ILocationDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _serviceProvider.GetRequiredService<EFLocationCreateDAL>(),
                ServiceFactoryType.Read => _serviceProvider.GetRequiredService<EFLocationReadDAL>(),
                ServiceFactoryType.Update => _serviceProvider.GetRequiredService<EFLocationUpdateDAL>(),
                ServiceFactoryType.Delete => _serviceProvider.GetRequiredService<EFLocationDeleteDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}