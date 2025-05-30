using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Clinics.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Clinics
{
    public class ClinicServiceFactory
    {
        private readonly IServiceProvider _provider;

        public ClinicServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IClinicDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Read => _provider.GetRequiredService<EFClinicReadDAL>(),
                ServiceFactoryType.Update => _provider.GetRequiredService<EFClinicUpdateDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}