using Buisness.Abstract;
using Entities.DTOs.BannerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _banner;
        public BannersController(IBannerService banner)
        {
            _banner = banner;
        }
        [Authorize(Roles = "Admin")]
        [SwaggerMultiGroup("admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(Create dto)
        {
            var response = await _banner.Create(dto, User.Identity.Name);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _banner.GetAll();
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _banner.Delete(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Update dto)
        {
            var response = await _banner.Update(dto, User.Identity.Name);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateData(Guid id)
        {
            var response = await _banner.UpdateData(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}