using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Emails
{
    public class EmailServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IWorkSpaceEmailDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _serviceProvider.GetRequiredService<EFWorkSpaceEmailCreateDAL>(),
                ServiceFactoryType.Custom => _serviceProvider.GetRequiredService<EFWorkSpaceEmailCustomDAL>(),
                ServiceFactoryType.Read => _serviceProvider.GetRequiredService<EFWorkSpaceEmailReadDAL>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}