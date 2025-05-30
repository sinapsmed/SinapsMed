using System.Security.Claims;
using Entities.DTOs.Helpers;
using Entities.Enums;

namespace WebApi.Services
{
    public class ControllerService
    {
        public static Superiority GetSuperiority(HttpContext context)
        {
            var user = context.User;
            if (user is null)
                return Superiority.Public;

            var value = user.Claims.FirstOrDefault(c => c.Type == "Superpiority")?.Value;

            if (value is null)
                return Superiority.Public;

            Enum.TryParse<Superiority>(value, out var periority);

            return periority;
        }

        public static ReqFrom? RequestFrom(HttpContext context)
        {
            var user = context.User;
            if (user is null)
                return null;

            var value = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var superiority = GetSuperiority(context);

            return new ReqFrom
            {
                Superiority = superiority,
                RequesterId = Guid.Parse(value),
                UserId = role == "User" ? value : null
            };
        }

        public static string GetModelEmail(HttpContext context)
        {
            var user = context.User;
            if (user is null)
                return default;

            var value = user.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;

            return value;
        }


        public static Guid GetModelId(HttpContext context)
        {
            var user = context.User;
            if (user is null)
                return default;

            var value = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Guid.TryParse(value, out var id);

            return id;
        }

        public static string GetUserId(HttpContext context)
        {
            var user = context.User;
            if (user is null)
                return "";

            var value = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return value;
        }

        public static string GetUserName(HttpContext context)
        {
            var user = context.User;
            if (user is null)
                return "";

            var value = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return value;
        }
    }
}