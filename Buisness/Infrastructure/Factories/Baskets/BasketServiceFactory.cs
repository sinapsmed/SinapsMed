using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Baskets
{
    public class BasketServiceFactory
    {
        private readonly IServiceProvider _provider;

        public BasketServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IBasketDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _provider.GetRequiredService<EFBasketCreateDAL>(),
                ServiceFactoryType.Read => _provider.GetRequiredService<EFBasketReadDAL>(),
                ServiceFactoryType.Update => _provider.GetRequiredService<EFBasketUpdateDAL>(),
                ServiceFactoryType.Delete => _provider.GetRequiredService<EFBasketDeleteDAL>(),
                ServiceFactoryType.Custom => _provider.GetRequiredService<EFBasketCustomDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}