using Buisness.Abstract;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.Enums.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsService _appointmentsService;
        public AppointmentsController(IAppointmentsService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        // [Authorize]
        [SwaggerMultiGroup("public")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddFile(IFormFile file, string title, Guid appointmentId)
        {
            var response = await _appointmentsService.AddFile(file, title, appointmentId);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        // [Authorize]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> AnamnezForm(Guid id)
        {
            var response = await _appointmentsService.AnamnezForm(id);
            return File(response.Data, "application/pdf", "AnamnezForm.pdf");
        }

        [Authorize]
        [HttpPost("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Create([FromBody] Create create)
        {
            var response = await _appointmentsService.Create(create);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        // [Authorize(Roles = "Expert")]
        [SwaggerMultiGroup("expert")]
        public async Task<IActionResult> AddAnamnezFormDiagnosis([FromBody] AnamnezCreate create)
        {
            var response = await _appointmentsService.AddAnamnezFormDiagnosis(create);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "User,Expert")]
        [SwaggerMultiGroup("public", "expert")]
        public async Task<IActionResult> Schedule(Guid? oponentId, Guid? serviceId, AppointmentStatus? status, int year, int month)
        {
            var reqFrom = ControllerService.RequestFrom(HttpContext);
            var response = await _appointmentsService.Schedule(reqFrom, oponentId, serviceId, status, year, month);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        // [Authorize(Roles = "Expert")]
        [SwaggerMultiGroup("expert")]
        public async Task<IActionResult> AppointmentFormData(Guid id)
        {
            var response = await _appointmentsService.AppointmentFormData(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [Authorize]
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var response = await _appointmentsService.Detail(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [Authorize]
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetAll(Guid? expertId, string? userId, int page = 1)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            var response = await _appointmentsService.GetAll(periority, expertId, userId, page);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        // [Authorize]
        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> Diagnoses(string search)
        {
            var response = await _appointmentsService.GetDiagnoses(search);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> NotifyExpert(Guid id)
        {
            var response = await _appointmentsService.NotifyExpert(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> UploadDiagnostics(IFormFile file)
        {
            var response = await _appointmentsService.UploadDiagnostics(file);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}