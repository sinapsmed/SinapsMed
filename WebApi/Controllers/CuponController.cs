using Buisness.Abstract;
using Entities.DTOs.CuponDtos;
using Entities.DTOs.CuponDtos.Body;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]

    public class CuponController : ControllerBase
    {
        private readonly ICuponService _service;

        public CuponController(ICuponService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Create([FromBody] Create create)
        {
            var response = await _service.Create(create);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> GetAll(int page = 1, int limit = 10)
        {
            var response = await _service.GetAll(page, limit);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> GetUsers([FromQuery] CuponUserBody body)
        {
            var response = await _service.GetUsers(body);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> GetServices([FromQuery] CuponServiceBody body)
        {
            var response = await _service.GetServices(body);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var response = await _service.GetById(id);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UsedCupons(Guid id, int page = 2)
        {
            var response = await _service.GetUsedCupons(id, page);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Delet(Guid id)
        {
            var response = await _service.Delete(id);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            var response = await _service.Update(update);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }
 
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        [Authorize]
        public async Task<IActionResult> Disount(string code, CuponType type)
        {
            var response = await _service.Discount(code, User.Identity.Name, type);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }
    }
}