using Buisness.Abstract;
using Entities.DTOs.StaffDtos.Body;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers.Support
{
    // [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("nam/[controller]")]
    public class CustomerSupportController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public CustomerSupportController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [SwaggerMultiGroup("staff")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddAsync([FromBody] StaffCreate create)
        {
            var response = await _staffService.AddAsync(create);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [SwaggerMultiGroup("staff")]
        [HttpGet("[action]")]
        public async Task<IActionResult> AllAsync(int page = 1)
        {
            var response = await _staffService.AllAsync(page);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [SwaggerMultiGroup("staff")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var response = await _staffService.DeleteAsync(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}