using Buisness.Abstract;
using Entities.Enums.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    [SwaggerMultiGroup("public")]
    public class OrdersController(IOrderService _service) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string unikalKey, int page = 1)
        {
            var superiority = ControllerService.GetSuperiority(HttpContext);
            var userName = ControllerService.GetUserName(HttpContext);
            var response = await _service.GetAll(unikalKey, userName, superiority, page);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CheckOutOrder(string? cupon)
        {
            var userId = ControllerService.GetUserId(HttpContext);
            var response = await _service.CheckOutOrder(userId, cupon);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Detailed(Guid id)
        {
            var response = await _service.Detailed(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Payment(int paymentId, PaymentStatus status)
        {
            var response = await _service.Payment(paymentId, status);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}