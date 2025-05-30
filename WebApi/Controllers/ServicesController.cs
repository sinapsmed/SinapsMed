using Buisness.Abstract;
using Entities.DTOs.Helpers;
using Entities.DTOs.ServiceDtos.Create;
using Entities.DTOs.ServiceDtos.Update;
using Entities.DTOs.SpecalitiyDtos.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]

    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _service;

        public ServicesController(IServiceService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Categories()
        {
            var response = await _service.GetCategories();
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddComplaint([FromBody] CreateComplaint createComplaint)
        {
            var response = await _service.AddComplaint(createComplaint);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComplaint(Guid id)
        {
            var response = await _service.DeleteComplaint(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetComplaints(Guid id)
        {
            var response = await _service.GetComplaints(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }


        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> AddPeriod([FromBody] PeriodDto period)
        {
            var response = await _service.AddPeriod(period);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> CreateCategory([FromBody] List<CreateCat> cats)
        {
            var response = await _service.CreateCategory(cats);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Header()
        {
            var response = await _service.GetHeaders();
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin", "expert")]
        public async Task<IActionResult> Periods(Guid servId)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            Guid.TryParse(User.Identity.Name, out var requesterId);
            var req = new ReqFrom { RequesterId = requesterId, Superiority = periority };
            var response = await _service.Periods(servId, req);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Services(Guid? categoryId, Guid? expertId)
        {
            var response = await _service.GetServices(categoryId, expertId);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> AllServices(Guid? expertId, int page = 1)
        {
            var response = await _service.AllServices(page, expertId);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var response = await _service.ServiceDetail(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }


        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> CreateService([FromBody] CreateSpecailty cats)
        {
            var response = await _service.AddService(cats);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Update([FromBody] ServiceUpdateGet update)
        {
            var response = await _service.Update(update);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UpdatePeriod([FromBody] ServicePeriodUpdateGet update)
        {
            var response = await _service.UpdatePeriod(update);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _service.Delete(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> DeletePeriod(Guid id)
        {
            var response = await _service.DeletePeriod(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UpdateServiceGet(Guid id)
        {
            var response = await _service.UpdateServiceData(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UpdateServicePeriodGet(Guid id)
        {
            var response = await _service.UpdateServicePeriodData(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UpdateCategoryGet(Guid id)
        {
            var response = await _service.UpdateCategoryData(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var response = await _service.DeleteCategory(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateGet updateGet)
        {
            var response = await _service.UpdateCategory(updateGet);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}