using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Accountatnts.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Accountants
{
    public class AccountantsServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AccountantsServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAccountantDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Read => _serviceProvider.GetRequiredService<EFAccountantReadDAL>(),
                ServiceFactoryType.Create => _serviceProvider.GetRequiredService<EFAccountantCreateDAL>(),
                ServiceFactoryType.Update => _serviceProvider.GetRequiredService<EFAccountantUpdateDAL>(),
                ServiceFactoryType.Delete => _serviceProvider.GetRequiredService<EFAccountantDeleteDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}