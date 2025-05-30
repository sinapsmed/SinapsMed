using Buisness.Abstract;
using Entities.DTOs.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]

    public class CustomController : ControllerBase
    {
        private readonly ICustomService _service;

        public CustomController(ICustomService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> SendOffer([FromBody] CreateOffer offer)
        {
            var response = await _service.Contact(offer);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> GetAll(int page = 1, int limit = 10)
        {
            var response = await _service.GetAll(page, limit);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _service.Delete(id);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Reply(Guid id, string message)
        {
            var response = await _service.Reply(id, message);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var response = await _service.MarkAsRead(id);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

    }
}