using System.Threading.RateLimiting;
using Buisness.Abstract;
using Buisness.Abstract.Admin;
using Buisness.Concrete;
using Buisness.Concrete.Admin;
using Buisness.Infrastructure.Factories.Accountants;
using Buisness.Infrastructure.Factories.Analyses;
using Buisness.Infrastructure.Factories.Appointments;
using Buisness.Infrastructure.Factories.Baskets;
using Buisness.Infrastructure.Factories.Blogs;
using Buisness.Infrastructure.Factories.Clinics;
using Buisness.Infrastructure.Factories.Cupon;
using Buisness.Infrastructure.Factories.Emails;
using Buisness.Infrastructure.Factories.Experts;
using Buisness.Infrastructure.Factories.Locations;
using Buisness.Infrastructure.Factories.Orders;
using Buisness.Infrastructure.Factories.Services;
using Buisness.Infrastructure.Factories.Staff;
using Buisness.Infrastructure.Factories.Users;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Abstract.Admin;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Concrete.SQLServer.EFDALs.Accountatnts;
using DataAccess.Concrete.SQLServer.EFDALs.Accountatnts.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Admins;
using DataAccess.Concrete.SQLServer.EFDALs.Analyses;
using DataAccess.Concrete.SQLServer.EFDALs.Analyses.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Appointments;
using DataAccess.Concrete.SQLServer.EFDALs.Appointments.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Auth;
using DataAccess.Concrete.SQLServer.EFDALs.Banners;
using DataAccess.Concrete.SQLServer.EFDALs.Baskets;
using DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Blogs;
using DataAccess.Concrete.SQLServer.EFDALs.Blogs.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Clinics;
using DataAccess.Concrete.SQLServer.EFDALs.Clinics.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Cupons;
using DataAccess.Concrete.SQLServer.EFDALs.Cupons.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Custom;
using DataAccess.Concrete.SQLServer.EFDALs.Emails;
using DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Experts;
using DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Locations;
using DataAccess.Concrete.SQLServer.EFDALs.Locations.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Orders;
using DataAccess.Concrete.SQLServer.EFDALs.Orders.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Partners;
using DataAccess.Concrete.SQLServer.EFDALs.Questions;
using DataAccess.Concrete.SQLServer.EFDALs.Services;
using DataAccess.Concrete.SQLServer.EFDALs.Services.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Staff;
using DataAccess.Concrete.SQLServer.EFDALs.Staff.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Users;
using DataAccess.Concrete.SQLServer.EFDALs.Users.CRUD;
using DataAccess.Profiles;
using DataAccess.Services;
using DataAccess.Services.Abstract;
using DataAccess.Services.Concrete;
using Entities.Concrete.UserEntities;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using StackExchange.Redis;

namespace Buisness.DependencyResolver
{
    public static class ServiceRegistration
    {
        public static void AddBuinessService(this IServiceCollection service)
        {
            service.AddScoped<AppDbContext>();
            service.AddAutoMapper(typeof(MappingProfile));

            service.AddScoped(typeof(IService), typeof(Manager));

            service.AddScoped<IAppointmentsService, AppointmentsManager>();
            service.AddScoped<IAuthService, AuthManager>();
            service.AddScoped<IAuthDAL, EFAuthDAL>();


            service.AddScoped<IAdminService, AdminManager>();
            service.AddScoped<IAdminDAL, EFAdminDAL>();

            service.AddScoped<IFileService, FileManager>();
            service.AddScoped<IDAtaAccessService, DataAccessService>();

            service.AddScoped<IBannerService, BannerManager>();
            service.AddScoped<IBannerDAL, EFBannerDAL>();

            service.AddScoped<IQuestionDAL, EFQuestionsDAL>();
            service.AddScoped<IQuestionsService, QuestionsManager>();

            service.AddScoped<IPartnerService, PartnerManager>();
            service.AddScoped<IPartnerDAL, EFPartnerDAL>();

            #region Appointment DI
            service.AddScoped<AppointmentAdapter>();
            service.AddScoped<EFAppointmentCreateDAL>();
            service.AddScoped<EFAppointmentCustomDAL>();
            service.AddScoped<EFAppointmentReadDAL>();
            #endregion

            #region WorkSpaceEmail DI
            service.AddScoped<IWorkSpaceService, WorkSpaceEmailManager>();
            service.AddScoped<WorkSpaceEmailAdapter>();
            service.AddScoped<EFWorkSpaceEmailReadDAL>();
            service.AddScoped<EmailServiceFactory>();
            service.AddScoped<EFWorkSpaceEmailCreateDAL>();
            service.AddScoped<EFWorkSpaceEmailCustomDAL>();
            #endregion

            #region Clinic DI
            service.AddScoped<ICLinicService, ClinicManager>();
            service.AddScoped<ClinicServiceFactory>();
            service.AddScoped<ClinicAdapter>();
            service.AddScoped<EFClinicReadDAL>();
            service.AddScoped<EFClinicUpdateDAL>();
            service.AddScoped<EFClinicReadDAL>();
            #endregion

            #region Cupon DI
            service.AddScoped<ICuponService, CuponManager>();
            service.AddScoped<CuponServiceFactory>();
            service.AddScoped<EFCuponCodeAdapter>();
            service.AddScoped<EFCuponReadDAL>();
            service.AddScoped<EFCuponCreateDAL>();
            service.AddScoped<EFCuponUpdateDAL>();
            service.AddScoped<EFCuponDeleteDAL>();
            #endregion

            #region Service DI
            service.AddScoped<IServiceService, ServiceManager>();
            service.AddScoped<ServicesFactory>();
            service.AddScoped<ServiceAdapter>();
            service.AddScoped<EFServiceCreateDAL>();
            service.AddScoped<EFServiceDeleteDAL>();
            service.AddScoped<EFServiceReadDAL>();
            service.AddScoped<EFServiceUpdateDAL>();
            #endregion

            #region Locations DI
            service.AddScoped<ILocationService, LocationManager>();
            service.AddScoped<LocationServiceFactory>();
            service.AddScoped<LocationsAdapter>();
            service.AddScoped<EFLocationCreateDAL>();
            service.AddScoped<EFLocationUpdateDAL>();
            service.AddScoped<EFLocationDeleteDAL>();
            service.AddScoped<EFLocationReadDAL>();
            #endregion

            #region Appointments DI
            service.AddScoped<IAppointmentsService, AppointmentsManager>();
            service.AddScoped<AppointmentServiceFactory>();
            service.AddScoped<AppointmentAdapter>();
            service.AddScoped<EFAppointmentCreateDAL>();
            service.AddScoped<EFAppointmentCustomDAL>();
            service.AddScoped<EFAppointmentReadDAL>();
            #endregion

            #region Users DI
            service.AddScoped<IUsersService, UsersManager>();
            service.AddScoped<UsersServiceFactory>();
            service.AddScoped<UsersAdapter>();
            service.AddScoped<EFUsersReadDAL>();
            #endregion

            #region Analses DI
            service.AddScoped<IAnalysisService, AnalysisManager>();
            service.AddScoped<AnalysesServiceFactory>();
            service.AddScoped<AnalysesAdapter>();
            service.AddScoped<EFAnalysisCreateDAL>();
            service.AddScoped<EFAnalysisDeleteDAL>();
            service.AddScoped<EFAnalysisReadDAL>();
            service.AddScoped<EFAnalysisUpdateDAL>();
            #endregion

            #region BasketDI
            service.AddScoped<IBasketService, BasketManager>();
            service.AddScoped<BasketServiceFactory>();
            service.AddScoped<BasketAdapter>();
            service.AddScoped<EFBasketCreateDAL>();
            service.AddScoped<EFBasketDeleteDAL>();
            service.AddScoped<EFBasketReadDAL>();
            service.AddScoped<EFBasketUpdateDAL>();
            service.AddScoped<EFBasketCustomDAL>();
            #endregion

            #region Accountant DI
            service.AddScoped<IAccountantService, AccountantManager>();
            service.AddScoped<AccountantsServiceFactory>();
            service.AddScoped<AccountantAdapter>();
            service.AddScoped<EFAccountantReadDAL>();
            service.AddScoped<EFAccountantUpdateDAL>();
            service.AddScoped<EFAccountantDeleteDAL>();
            service.AddScoped<EFAccountantCreateDAL>();
            #endregion

            #region Staff DI
            service.AddScoped<IStaffService, StaffManager>();
            service.AddScoped<StaffServiceFactory>();
            service.AddScoped<StaffAdapter>();
            service.AddScoped<EFStaffReadDAL>();
            service.AddScoped<EFStaffCreateDAL>();
            service.AddScoped<EFStaffDeleteDAL>();
            #endregion

            #region  Order DI
            service.AddScoped<IOrderService, OrderManager>();
            service.AddScoped<OrderServiceFactory>();
            service.AddScoped<OrderAdapter>();
            service.AddScoped<EFOrderCreateDAL>();
            service.AddScoped<EFOrderReadDAL>();
            #endregion

            #region  Blog DI
            service.AddScoped<IBlogService, BlogManager>();
            service.AddScoped<BlogServiceFactory>();
            service.AddScoped<BlogAdapter>();
            service.AddScoped<EFBlogCreateDAL>();
            service.AddScoped<EFBlogReadDAL>();
            service.AddScoped<EFBlogUpdateDAL>();
            service.AddScoped<EFBlogDeleteDAL>();
            #endregion

            service.AddScoped<ICustomService, CustomManager>();
            service.AddScoped<ICustomDAL, EFCustomDAL>();

            #region Expert DI
            service.AddScoped<IExpertService, ExpertManager>();
            service.AddScoped<ExpertServiceFactory>();
            service.AddScoped<ExpertAdapter>();
            service.AddScoped<EFExpertCreateDAL>();
            service.AddScoped<EFExpertReadDAL>();
            service.AddScoped<EFExpertUpdateDAL>();
            service.AddScoped<EFExpertDeleteDAL>();
            service.AddScoped<EFExpertCustomDAL>();
            #endregion

            #region Redis 
            service.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("127.0.0.1:6379"));
            service.AddSingleton<IRedisCacheService, RedisCacheService>();
            #endregion

            #region HangFire
            service.AddHangfire(config => config.UseMemoryStorage());
            service.AddHangfireServer();
            #endregion

            #region Google
            service.AddScoped<IGoogleService, GoogleManager>();
            #endregion


            #region Localizations 
            service.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            service.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            #endregion

            service.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Request.Headers.Host.ToString(), partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            AutoReplenishment = true,
                            Window = TimeSpan.FromSeconds(1)
                        });
                });
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later... ", cancellationToken: token);
                };
            });

            service.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                options.Lockout.MaxFailedAccessAttempts = 5;

            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddErrorDescriber<IdentityDescriberLoclaizerMiddleware>()
                .AddDefaultTokenProviders();
        }

    }
}
