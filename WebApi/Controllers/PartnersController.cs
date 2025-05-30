using Buisness.Abstract;
using Entities.DTOs.PartnerDtos;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]

    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _service;
        public PartnersController(IPartnerService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAll();
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Create(Create create)
        {
            var response = await _service.Create(create);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _service.Delete(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Update(Update update)
        {
            var response = await _service.Update(update);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}