using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Analyses.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Analyses
{
    public class AnalysesServiceFactory
    {

        private readonly IServiceProvider _serviceProvider;

        public AnalysesServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAnalysisDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _serviceProvider.GetRequiredService<EFAnalysisCreateDAL>(),
                ServiceFactoryType.Read => _serviceProvider.GetRequiredService<EFAnalysisReadDAL>(),
                ServiceFactoryType.Update => _serviceProvider.GetRequiredService<EFAnalysisUpdateDAL>(),
                ServiceFactoryType.Delete => _serviceProvider.GetRequiredService<EFAnalysisDeleteDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}