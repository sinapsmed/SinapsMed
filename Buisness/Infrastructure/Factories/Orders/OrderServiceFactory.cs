using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Orders.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Orders
{
    public class OrderServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderServiceFactory(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public IOrderDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _serviceProvider.GetRequiredService<EFOrderCreateDAL>(),
                ServiceFactoryType.Read => _serviceProvider.GetRequiredService<EFOrderReadDAL>(),
                // ServiceFactoryType.Update => _serviceProvider.GetRequiredService<>(),
                // ServiceFactoryType.Delete => _serviceProvider.GetRequiredService<>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}