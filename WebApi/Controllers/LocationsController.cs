using Buisness.Abstract;
using Entities.DTOs.LocationDtos;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]

    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _service;
        public LocationsController(ILocationService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> City([FromBody] CreateCity createCity)
        {
            var result = await _service.AddCity(createCity);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }
        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Region([FromBody] CreateRegion createRegion)
        {
            var result = await _service.AddRegion(createRegion);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Village([FromBody] CreateVillage createVillage)
        {
            var result = await _service.AddVillage(createVillage);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Clinic([FromBody] CreateClinic createClinic)
        {
            var result = await _service.AddClinic(createClinic);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Cities(int page = 1, int limit = 10)
        {
            var result = await _service.Cities(page, limit);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Regions(Guid id, int page = 1, int limit = 10)
        {
            var result = await _service.Regions(id, page, limit);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Villages(Guid cityId, Guid? regionId, int page = 1, int limit = 10)
        {
            var result = await _service.Villages(cityId, regionId, page, limit);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> VillagesDetailed(Guid? regionId, int page = 1, int limit = 10)
        {
            var result = await _service.Villages(regionId, page, limit);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Clinics(string? location, Guid? cityId, Guid? regionId, Guid? villageId, Guid? partnerId, int page = 1, int limit = 10)
        {
            var result = await _service.Clinics(location, cityId, regionId, villageId, partnerId, page, limit);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> ClinicsDetailed(Guid? villageId, int page = 1, int limit = 10)
        {
            var result = await _service.Clinics(villageId, page, limit);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> City(Guid id)
        {
            var result = await _service.DeleteCity(id);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }
        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Region(Guid id)
        {
            var result = await _service.DeleteRegion(id);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }
        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Clinic(Guid id)
        {
            var result = await _service.DeleteClinic(id);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }
        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Village(Guid id)
        {
            var result = await _service.DeleteVillage(id);
            return new ObjectResult(result) { Value = result, StatusCode = (int)result.StatusCode };
        }
    }
}