using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.EFDALs.Appointments.CRUD;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness.Infrastructure.Factories.Appointments
{
    public class AppointmentServiceFactory
    {
        private readonly IServiceProvider _provider;

        public AppointmentServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        public IAppointmentDAL GetService(ServiceFactoryType type)
        {
            return type switch
            {
                ServiceFactoryType.Create => _provider.GetRequiredService<EFAppointmentCreateDAL>(),
                ServiceFactoryType.Read => _provider.GetRequiredService<EFAppointmentReadDAL>(),
                // ServiceFactoryType.Update => _provider.GetRequiredService<EFAppointmentUpdateDAL>(),
                // ServiceFactoryType.Delete => _provider.GetRequiredService<EFAppointmentDeleteDAL>(),
                ServiceFactoryType.Custom => _provider.GetRequiredService<EFAppointmentCustomDAL>(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}