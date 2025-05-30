using System.Reflection;
using Entities.Common;
using Entities.Concrete.AccountantEntities;
using Entities.Concrete.Admin;
using Entities.Concrete.Analyses;
using Entities.Concrete.Appointments;
using Entities.Concrete.Banner;
using Entities.Concrete.BasketEntities;
using Entities.Concrete.BlogEntities;
using Entities.Concrete.Clinics;
using Entities.Concrete.CuponCodes;
using Entities.Concrete.Emails;
using Entities.Concrete.Experts;
using Entities.Concrete.Experts.WorkTimes;
using Entities.Concrete.Faq;
using Entities.Concrete.Forms;
using Entities.Concrete.Forms.Diagnoses;
using Entities.Concrete.Helpers;
using Entities.Concrete.Locations;
using Entities.Concrete.OrderEntities;
using Entities.Concrete.Partners;
using Entities.Concrete.Services;
using Entities.Concrete.Staff;
using Entities.Concrete.UserEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrete.SQLServer.DataBase
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebApi"))
                    .AddEnvironmentVariables();

                var configuration = builder.Build();

                var connectionString = configuration["ConnectionStrings:Default"];

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        #region DbSets 
        public DbSet<Support> Supports { get; set; }

        public DbSet<Accountant> Accountants { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<WorkSpaceEmail> Emails { get; set; }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<BannerLang> BannerLangs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogLang> Bloglangs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogCategoryLang> BlogCategoryLangs { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionLang> QuestionLangs { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReply> Replies { get; set; }

        public DbSet<Expert> Experts { get; set; }
        public DbSet<WrokIntervalHoursData> WrokIntervals { get; set; }
        public DbSet<WorkRoutine> WorkRoutines { get; set; }
        public DbSet<WorkRoutineWeekDay> WorkRoutineWeekDays { get; set; }
        public DbSet<WorkHoliday> WorkHolidays { get; set; }
        public DbSet<WorkPause> WorkPauses { get; set; }
        public DbSet<ExpertServicePeriod> ExpertPeriods { get; set; }

        public DbSet<Service> Services { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ServiceLang> ServiceLanguages { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ServiceCategoryLang> ServiceCategoryLanguages { get; set; }
        public DbSet<ServicePeriod> ServicePeriods { get; set; }
        public DbSet<ServicePeriodLang> ServicePeriodLanguages { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AdditionalUser> AdditionalUsers { get; set; }
        public DbSet<AppointmentAttachment> Attachments { get; set; }
        public DbSet<AnamnezForm> Forms { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }


        public DbSet<Admin> Admins { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Village> Villages { get; set; }

        public DbSet<Clinic> Clinics { get; set; }

        public DbSet<Analysis> Analyses { get; set; }
        public DbSet<AnalysisCategory> AnalysisCategories { get; set; }
        public DbSet<Offer> Offers { get; set; }


        public DbSet<Cupon> Cupons { get; set; }
        public DbSet<CuponUsing> UsedCupons { get; set; }
        public DbSet<SpesficCuponUser> SpesficCuponUsers { get; set; }
        public DbSet<SpesficServiceCupon> SpesficServiceCupons { get; set; }

        public DbSet<Log> Logs { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<AnamnezFormDiagnosis>()
                .HasKey(af => new { af.AnamnezFormId, af.DiagnosisId });

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Basket)
                .WithOne(b => b.User)
                .HasForeignKey<Basket>(b => b.UserId);

            modelBuilder.Entity<AnamnezFormDiagnosis>()
                .HasOne(af => af.AnamnezForm)
                .WithMany(a => a.AnamnezFormDiagnoses)
                .HasForeignKey(af => af.AnamnezFormId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnamnezFormDiagnosis>()
                .HasOne(af => af.Diagnosis)
                .WithMany(d => d.AnamnezFormDiagnoses)
                .HasForeignKey(af => af.DiagnosisId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.AdditionalUser)
                .WithOne()
                .HasForeignKey<Appointment>(a => a.AdditionalUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Clinic>()
                .HasOne(c => c.Email)
                .WithMany()
                .HasForeignKey(c => c.EmailId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<WorkRoutine>()
                .HasOne(wr => wr.Expert)
                .WithOne(e => e.Routine)
                .HasForeignKey<WorkRoutine>(wr => wr.ExpertId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkRoutine>()
                .HasMany(wr => wr.DayOfWeeks)
                .WithOne(dow => dow.WorkRoutine)
                .HasForeignKey(dow => dow.WorkRoutineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpertServicePeriod>()
                .HasOne(esp => esp.Expert)
                .WithMany(e => e.ServicePeriods)
                .HasForeignKey(esp => esp.ExpertId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpertServicePeriod>()
                .HasOne(esp => esp.ServicePeriod)
                .WithMany(sp => sp.ExpertPeriods)
                .HasForeignKey(esp => esp.ServicePeriodId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpesficCuponUser>()
                .HasOne(c => c.Cupon)
                .WithMany(c => c.SpesficCuponUsers)
                .HasForeignKey(c => c.CuponId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpesficServiceCupon>()
                .HasOne(c => c.Cupon)
                .WithMany(c => c.SpesficServiceCupons)
                .HasForeignKey(c => c.CuponId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CuponUsing>()
                .HasOne(c => c.Cupon)
                .WithMany(c => c.UsedCupons)
                .HasForeignKey(c => c.CuponId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceCategoryLang>()
                .HasOne(cl => cl.Category)
                .WithMany(c => c.Languages)
                .HasForeignKey(cl => cl.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BannerLang>()
                .HasOne(bl => bl.Banner)
                .WithMany(b => b.Languages)
                .HasForeignKey(bl => bl.BannerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Blogs)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogCategoryLang>()
                .HasOne(bcl => bcl.Category)
                .WithMany(bc => bc.Languages)
                .HasForeignKey(bcl => bcl.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogLang>()
                .HasOne(bl => bl.Blog)
                .WithMany(b => b.Languages)
                .HasForeignKey(bl => bl.BlogId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<QuestionLang>()
                .HasOne(bl => bl.Question)
                .WithMany(b => b.Languages)
                .HasForeignKey(bl => bl.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Region>()
                .HasOne(r => r.City)
                .WithMany(c => c.Regions)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Village>()
                .HasOne(v => v.Region)
                .WithMany(r => r.Villages)
                .HasForeignKey(v => v.RegionId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.UtcNow;
                        data.Entity.CreatedBy = "Sinapsmed.com" ?? data.Entity.CreatedBy;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.UtcNow;
                        data.Entity.UpdatedBy = "Sinapsmed.com" ?? data.Entity.UpdatedBy;
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.UtcNow;
                        data.Entity.CreatedBy = "Sinapsmed.com" ?? data.Entity.CreatedBy;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.UtcNow;
                        data.Entity.UpdatedBy = "Sinapsmed.com" ?? data.Entity.UpdatedBy;
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChanges();
        }

    }
}
