using Buisness.Abstract;
using Core.Helpers.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Services.Abstract;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    public class AnalysesController : ControllerBase
    {
        private readonly IAnalysisService _service;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _email;
        private readonly IDAtaAccessService _serv;
        public AnalysesController(IAnalysisService service, IEmailService email, IWebHostEnvironment env, IDAtaAccessService serv)
        {
            _service = service;
            _email = email;
            _env = env;
            _serv = serv;
        }


        [HttpGet("[action]")]
        [SwaggerMultiGroup("public", "admin")]
        public async Task<IActionResult> GetAll([FromQuery] Filter filter)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            if (periority is Superiority.Admin)
            {
                var response = await _service.GetAllDetailed(filter);
                return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
            }
            else
            {
                var response = await _service.GetAll(filter);
                return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
            }
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public", "admin")]
        public async Task<IActionResult> GetCats()
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            if (periority is Superiority.Admin)
            {

                var response = await _service.GetDetailedCats();
                return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
            }
            else
            {
                var response = await _service.GetCats();
                return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
            }
        }
        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Add([FromBody] Create create)
        {
            var response = await _service.Add(create);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> AddCat([FromBody] CreateCategory create)
        {
            var response = await _service.AddCat(create);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> AddList(IFormFile file, string agentMail)
        {
            var response = await _service.AddList(file, agentMail);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> DeleteCat(Guid id)
        {
            var response = await _service.DeleteCat(id);
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
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            var response = await _service.Update(update);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UpdateCat([FromBody] UpdateCategory update)
        {
            var response = await _service.UpdateCat(update);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

    }
}