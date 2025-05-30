using Buisness.Abstract;
using Entities.DTOs.ExpertDtos;
using Microsoft.AspNetCore.Mvc;
using Entities.Enums;
using WebApi.Services.Swagger;
using WebApi.Services;
using Entities.DTOs.ExpertDtos.BodyDtos;
using Entities.DTOs.ExpertDtos.GetDtos;
using Microsoft.AspNetCore.Authorization;
using Entities.DTOs.ServiceDtos.Get;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    public class ExpertsController : ControllerBase
    {
        private readonly IExpertService _service;

        public ExpertsController(IExpertService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> AddExpert([FromBody] Create create)
        {
            var response = await _service.AddExpert(create);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> AddPeriod(Guid expertId, double price, Guid periodId)
        {
            var response = await _service.AddPeriod(expertId, price, periodId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> UpdateWorkRoutine([FromBody] WorkRoutineUpdate routineUpdate)
        {
            var response = await _service.UpdateWorkRoutine(routineUpdate);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> AddServices([FromBody] List<Guid> serviceIds, Guid expertId)
        {
            var response = await _service.AddServices(expertId, serviceIds);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> AddHoliday([FromBody] CreateWorkHoliday createWorkHoliday)
        {
            var response = await _service.AddHoliday(createWorkHoliday);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin", "expert")]
        public async Task<IActionResult> AddPause([FromBody] CreateWorkPause createWorkPause)
        {
            var response = await _service.AddPause(createWorkPause);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin", "expert")]
        public async Task<IActionResult> ExpertPeriods(Guid expertId, Guid? serviceId)
        {
            var response = await _service.ExpertPeriods(expertId, serviceId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin", "public")]
        public async Task<IActionResult> GetAll(Guid? serviceId, string? search, int page = 1, int limit = 10)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);

            if (periority is Superiority.Admin)
            {
                var response = await _service.GetAllDetailed(serviceId, search, page, limit);
                return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
            }
            else
            {
                var response = await _service.GetAll(serviceId, search, page, limit);
                return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
            }
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> WorkingHours(DateTime date, Guid expertId)
        {
            var response = await _service.WorkingHours(date, expertId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> UpdateExpertPeriod([FromBody] ExpertPeriodGet periodGet)
        {
            var response = await _service.UpdateExpertPeriod(periodGet);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> DeleteExpertPeriod(Guid expertId, Guid servicePeriodId)
        {
            var response = await _service.DeleteExpertPeriod(expertId, servicePeriodId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert")]
        public async Task<IActionResult> ExpertCalendar(Guid expertId, int year, byte month)
        {
            var response = await _service.GetExpertCalendar(expertId, year, month);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert")]
        public async Task<IActionResult> UpdateWorkRoutineData(Guid expertId)
        {
            var response = await _service.UpdateWorkRoutineData(expertId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> DeleteService(Guid expertId, Guid serviceId)
        {
            var response = await _service.DeleteService(expertId, serviceId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> WorkingDays(Guid expertId)
        {
            var response = await _service.WorkingDays(expertId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> Services(Guid expertId, Guid? serviceId)
        {
            var response = await _service.Services(expertId, serviceId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert", "public")]
        public async Task<IActionResult> GetBoosted(int page = 1, int limit = 10)
        {
            page = page <= 0 ? 1 : page;
            var response = await _service.GetBoosted(limit, page);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [Authorize]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> UpdateData(Guid expertId)
        {
            var response = await _service.UpdateData(expertId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [Authorize]
        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> Appointments(Guid expertId, int page = 1)
        {
            var response = await _service.Appointments(expertId, page);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [Authorize]
        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> AppointmentDetailed(Guid id)
        {
            var response = await _service.AppointmentDetailed(id);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [Authorize]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> Update([FromBody] UpdateData update)
        {
            var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _service.Update(update, ipAddress);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        public async Task<IActionResult> UpdatePassword(Guid expertId, string oldPassword, string newPassword)
        {
            var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _service.UpdatePassword(expertId, oldPassword, newPassword, ipAddress);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Expert,Staff")]
        [SwaggerMultiGroup("expert")]
        public async Task<IActionResult> AddItemToUserBasket(string userId, List<Guid> anlayses)
        {
            var response = await _service.AddItemToUserBasket(userId, anlayses);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }
    }
}