using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Cupons.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Cupon
{
    public class CuponServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CuponServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICuponDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _serviceProvider.GetRequiredService<EFCuponCreateDAL>(),
                ServiceFactoryType.Read => _serviceProvider.GetRequiredService<EFCuponReadDAL>(),
                ServiceFactoryType.Update => _serviceProvider.GetRequiredService<EFCuponUpdateDAL>(),
                ServiceFactoryType.Delete => _serviceProvider.GetRequiredService<EFCuponDeleteDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}