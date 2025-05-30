using Buisness.Abstract;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers.ClinicLevel
{
    [ApiController]
    [Route("nam/[controller]")]
    public class ClinicsController : ControllerBase
    {
        private readonly ICLinicService _service;

        public ClinicsController(ICLinicService service)
        {
            _service = service;
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("clinic")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> CheckAsUsed(Guid itemId)
        {
            var clinicId = ControllerService.GetModelId(HttpContext);
            var response = await _service.CheckAsUsed(clinicId, itemId);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("clinic")]
        [Authorize(Roles = "Clinic,Staff,Admin")]
        public async Task<IActionResult> Orders(int page = 1)
        {
            Guid? clinicId = null;
            var superiority = ControllerService.GetSuperiority(HttpContext);

            if (superiority is Superiority.Clinic)
                clinicId = ControllerService.GetModelId(HttpContext);

            var response = await _service.Orders(clinicId, page);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("clinic")]
        [Authorize(Roles = "Clinic")]
        public async Task<IActionResult> ItemDetail(Guid? itemId, string? code)
        {
            var clinicId = ControllerService.GetModelId(HttpContext);
            var response = await _service.ItemDetail(clinicId, itemId, code);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("clinic", "admin")]
        public async Task<IActionResult> ClinicDetail(Guid? clinicId)
        {
            clinicId = clinicId ?? ControllerService.GetModelId(HttpContext);
            var response = await _service.ClinicDetail(Guid.Parse(clinicId.ToString()));
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}