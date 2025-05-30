using System.Text;
using Hangfire.Dashboard;

namespace WebApi.Services.HangfireService
{
    public class AuthService : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            string username = "holaadmin";
            string password = "HolaWorld";

            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                string usernamePassword = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int separatorIndex = usernamePassword.IndexOf(':');

                var providedUsername = usernamePassword.Substring(0, separatorIndex);
                var providedPassword = usernamePassword.Substring(separatorIndex + 1);

                return providedUsername == username && providedPassword == password;
            }

            httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
            httpContext.Response.StatusCode = 401;
            return false;
        }
    }


    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public SwaggerBasicAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];

                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    string usernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    int separatorIndex = usernamePassword.IndexOf(':');

                    string username = usernamePassword.Substring(0, separatorIndex);
                    string password = usernamePassword.Substring(separatorIndex + 1);

                    if (username == _config["Swagger:UserName"] && password == _config["Swagger:Password"])
                    {
                        await _next.Invoke(context);
                        return;
                    }
                }

                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }
    }

}