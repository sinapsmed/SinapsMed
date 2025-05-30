using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Users.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Users
{
    public class UsersServiceFactory
    {
        private readonly IServiceProvider _provider;

        public UsersServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IUsersDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Read => _provider.GetRequiredService<EFUsersReadDAL>(),
                _ => throw new NotImplementedException()
            };
        }

    }
}