using Buisness.Abstract;
using Entities.DTOs.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers.Email
{
    [ApiController]
    [Route("nam/[controller]")]
    [SwaggerMultiGroup("workspace")]

    public class EmailController : ControllerBase
    {
        private readonly IWorkSpaceService _emailService;

        public EmailController(IWorkSpaceService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CheckPassWordAsync(string email, string pasword)
        {
            var response = await _emailService.CheckPassWordAsync(email, pasword);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "WorkSpace")]
        public async Task<IActionResult> Messages(int page = 1)
        {
            var email = ControllerService.GetModelEmail(HttpContext);
            var response = await _emailService.GetMessages(email, page);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "WorkSpace")]
        public async Task<IActionResult> SendEmailAsync([FromBody] SendMessage message)
        {
            var id = ControllerService.GetModelId(HttpContext);
            var response = await _emailService.SendEmailAsync(id, message.To, message.Title, message.Content);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}