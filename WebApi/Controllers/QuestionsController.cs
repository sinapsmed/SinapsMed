using Buisness.Abstract;
using Entities.DTOs.QuestionDtos;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [Route("nam/[controller]")]
    [ApiController]

    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsService _service;
        public QuestionsController(IQuestionsService service)
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
        public async Task<IActionResult> Create(List<Create> dto)
        {
            var response = await _service.Create(dto);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var response = await _service.Delete(Id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}