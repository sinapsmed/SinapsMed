using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Experts
{
    public class ExpertServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpertServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IExpertDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _serviceProvider.GetRequiredService<EFExpertCreateDAL>(),
                ServiceFactoryType.Read => _serviceProvider.GetRequiredService<EFExpertReadDAL>(),
                ServiceFactoryType.Update => _serviceProvider.GetRequiredService<EFExpertUpdateDAL>(),
                ServiceFactoryType.Delete => _serviceProvider.GetRequiredService<EFExpertDeleteDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}