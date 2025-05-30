using Buisness.Abstract;
using Entities.Enums.File;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [Route("nam/[controller]")]
    [ApiController]
    [SwaggerMultiGroup("admin")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _file;

        public FilesController(IFileService file)
        {
            _file = file;
        }
 
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(string url)
        {
            var response = await _file.Delete(url);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(IFormFile file, FileCategory category, FileType type)
        {
            var response = await _file.Create(file, category, type);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode };
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(string url, IFormFile file, FileCategory category)
        {
            var response = await _file.Update(url, file, category);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode };
        }
    }
}