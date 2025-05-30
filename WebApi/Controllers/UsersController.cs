using Buisness.Abstract;
using Entities.DTOs.AuthDtos;
using Entities.Enums.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;
        private readonly IAuthService _auth;
        public UsersController(IUsersService service, IAuthService auth)
        {
            _service = service;
            _auth = auth;
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AllUsers(string? search, int page = 1, int limit = 10)
        {
            var response = await _service.Users(page, limit, search);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        // [Authorize(Roles = "User")]
        public async Task<IActionResult> UserAppointments(string userId, AppointmentStatus? status, int page = 1)
        {
            var response = await _service.UserAppointments(userId, status, page);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserAnlyses(string userId, int page = 1)
        {
            var response = await _service.GetUserAnlysis(userId, page);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        // [Authorize(Roles = "User")]
        public async Task<IActionResult> ApointmentDetail(Guid appointmentId)
        {
            var response = await _service.ApointmentDetail(appointmentId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("public")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update(UserInfo update)
        {
            var response = await _auth.UpdateUser(update);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
        [HttpPost("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Register(Register register)
        {
            var response = await _auth.Register(register);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [Authorize]
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> UserInfo(string userId)
        {
            var response = await _auth.UserInfo(userId);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}