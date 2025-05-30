using Entities.Concrete.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.SQLServer.DataBase.Configurations.Partners
{
    public class PartnersConfigurations : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Number)
                .ValueGeneratedOnAdd()
                .HasAnnotation("Npgsql:IdentityOptions", "1,1");
        }
    }
}