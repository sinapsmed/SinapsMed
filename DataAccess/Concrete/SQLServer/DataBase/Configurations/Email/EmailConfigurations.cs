using Entities.Concrete.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.SQLServer.DataBase.Configurations.Email
{
    public class EmailConfigurations : IEntityTypeConfiguration<WorkSpaceEmail>
    {
        public void Configure(EntityTypeBuilder<WorkSpaceEmail> builder)
        {
            builder.HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}