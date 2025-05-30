using System.Globalization;
using System.Text;
using Buisness.DependencyResolver;
using Core.DependencyResolver;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Configurations;
using WebApi.Filters;
using WebApi.Middlewares;
using WebApi.Services.HangfireService;
using WebApi.Services.Swagger;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options => options.Filters.Add(new LoggingFilter()))
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddBuinessService();

builder.Services.AddCoreService();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        };
    });



builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("admin", new OpenApiInfo
        {
            Title = "Admin API",
            Version = "v1"
        });

        c.SwaggerDoc("workspace", new OpenApiInfo
        {
            Title = "WorkSpace API",
            Version = "v1"
        });

        c.SwaggerDoc("staff", new OpenApiInfo
        {
            Title = "Staff API",
            Version = "v1"
        });

        c.SwaggerDoc("clinic", new OpenApiInfo
        {
            Title = "Clinic API",
            Version = "v1"
        });

        c.SwaggerDoc("accountant", new OpenApiInfo
        {
            Title = "Accountant API",
            Version = "v1"
        });

        c.SwaggerDoc("public", new OpenApiInfo
        {
            Title = "Public API",
            Version = "v1"
        });

        c.SwaggerDoc("expert", new OpenApiInfo
        {
            Title = "Expert API",
            Version = "v1"
        });

        c.DocInclusionPredicate((docName, apiDesc) =>
          {
              var multiGroupAttribute = apiDesc.ActionDescriptor
                  .EndpointMetadata
                  .OfType<SwaggerMultiGroupAttribute>()
                  .FirstOrDefault();

              if (multiGroupAttribute != null)
              {
                  if (multiGroupAttribute.GroupNames.Count() == 0)
                  {
                      return true;
                  }

                  return multiGroupAttribute.GroupNames.Contains(docName);
              }

              return false;
          });


        c.CustomSchemaIds(type => type.FullName);
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
builder.Services.AddCors(c =>
    {
        c.AddPolicy("SinapsCors", c =>
        {
            c.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
        });
    });
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = LocalizationConfig.GetSupportedCultures();
        options.AddSupportedCultures(supportedCultures);
        options.AddSupportedUICultures(supportedCultures);

        options.FallBackToParentUICultures = true;

        options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
        {
            var defaultCulture = new CultureInfo(LocalizationConfig.GetDefaultCulture());
            return new ProviderCultureResult(defaultCulture.Name);
        }));
    });


var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/public/swagger.json", "Public API");
    c.SwaggerEndpoint("/swagger/workspace/swagger.json", "WorkSpace API");
    c.SwaggerEndpoint("/swagger/staff/swagger.json", "Staff API");
    c.SwaggerEndpoint("/swagger/clinic/swagger.json", "Clinic API");
    c.SwaggerEndpoint("/swagger/accountant/swagger.json", "Accountant API");
    c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API");
    c.SwaggerEndpoint("/swagger/expert/swagger.json", "Expert API");
    c.DefaultModelsExpandDepth(-1);
});

app.UseRouting();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new AuthService() }
});

app.UseHangfireServer();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("SinapsCors");

var supportedCultures = LocalizationConfig.GetSupportedCultures();
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(LocalizationConfig.GetDefaultCulture()),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};
app.UseRequestLocalization(localizationOptions);
app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<SwaggerBasicAuthMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseStaticFiles();

app.Run();
