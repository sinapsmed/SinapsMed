using Entities.Concrete.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.SQLServer.DataBase.Configurations.Appointments
{
    public class AppointmentsConfigurations : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.Property(c => c.Number)
                .ValueGeneratedOnAdd()
                .HasAnnotation("Npgsql:IdentityOptions", "1,1");
        }
    }
}