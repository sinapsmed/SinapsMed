using Entities.Concrete.Analyses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.SQLServer.DataBase.Configurations.Analyses
{
    public class AnalysisCategoryConfiguration : IEntityTypeConfiguration<AnalysisCategory>
    {
        public void Configure(EntityTypeBuilder<AnalysisCategory> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(x => x.Number)
                .ValueGeneratedOnAdd()
                .HasAnnotation("Npgsql:IdentityOptions", "1,1");
        }
    }
}