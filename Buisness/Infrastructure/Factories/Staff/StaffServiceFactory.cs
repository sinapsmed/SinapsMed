using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Staff.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Staff
{
    public class StaffServiceFactory
    {
        private readonly IServiceProvider _provider;

        public StaffServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IStaffDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Read => _provider.GetRequiredService<EFStaffReadDAL>(),
                ServiceFactoryType.Delete => _provider.GetRequiredService<EFStaffDeleteDAL>(),
                ServiceFactoryType.Create => _provider.GetRequiredService<EFStaffCreateDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}