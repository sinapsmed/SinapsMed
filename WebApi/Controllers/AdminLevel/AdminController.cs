using Buisness.Abstract;
using Buisness.Abstract.Admin;
using Entities.DTOs.Admin;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers.AdminLevel
{
    [ApiController]
    // [ApiExplorerSettings(IgnoreApi = true)]
    [Route("nam/eb779d77-f691-431f-a5ac-46a364c2640a/[controller]")]
    [SwaggerMultiGroup("admin")]
    public class AdminController(IAdminService service, IAuthService authService) : ControllerBase
    {
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin([FromBody] Create create)
        {
            var response = await service.AddAdmin(create);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReloadRole(Guid id, string password, Superiority superiority)
        {
            var response = await service.ReloadAdmin(id, password,superiority);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdmins()
        {
            var response = await service.GetAll();
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(Superiority superiority, string loginId, string password)
        {
            Core.Utilities.Results.Abstract.IResult response;
            if (superiority == Superiority.Admin)
            {
                response = await service.Login(loginId, password);
            }
            else
            {
                response = await authService.CommonLogin(superiority, loginId, password);
            }
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> OTP(string loginId, long otp)
        {
            var response = await authService.OTP(loginId, otp);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }
    }
}