using Buisness.Abstract;
using DataAccess.Services.Abstract;
using Entities.DTOs.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    [SwaggerMultiGroup("public")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly RoleManager<IdentityRole> _role;
        private readonly IGoogleService _google;
        private readonly IConfiguration _config;
        public AuthController(IAuthService auth, RoleManager<IdentityRole> role, IGoogleService google, IConfiguration config)
        {
            _auth = auth;
            _role = role;
            _google = google;
            _config = config;
        }

        [HttpGet("/auth/auth-callback")]
        public async Task<IActionResult> AuthCallback(string code)
        {
            var response = await _google.AuthorizeUser(code);

            if (response.Success)
                return Redirect($"{_config["App:BaseLink"]}");
            else
                return Redirect($"{_config["App:BaseLink"]}/problem");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await _auth.ForgotPassword(email);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            changePassword.UserName = User.Identity.Name;
            var response = await _auth.ChangePassword(changePassword);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(string email, string token, string resetPassword)
        {
            var response = await _auth.ResetPassword(email, token, resetPassword);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PhoneOtpAgain(string phone, string email)
        {
            var response = await _auth.PhoneOtpAgain(phone, email);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdatePhone(string phone, string email)
        {
            var response = await _auth.PhoneOtpAgain(phone, email);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> VerifyPhone(string mail, string otp)
        {
            var response = await _auth.VerifyPhone(mail, otp);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var response = await _auth.Login(new Login { Email = email, Password = password });
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRoles()
        {
            await _role.CreateAsync(new IdentityRole("User"));
            await _role.CreateAsync(new IdentityRole("Admin"));
            await _role.CreateAsync(new IdentityRole("Expert"));
            await _role.CreateAsync(new IdentityRole("Clinic"));
            await _role.CreateAsync(new IdentityRole("WorkSpace"));
            await _role.CreateAsync(new IdentityRole("Accountant"));
            return Ok();
        }
    }
}