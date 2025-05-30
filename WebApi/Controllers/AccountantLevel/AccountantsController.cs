using Buisness.Abstract;
using Entities.DTOs.AccountantDtos;
using Entities.Enums.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers.AccountantLevel
{
    [ApiController]
    [Route("nam/[controller]")]
    public class AccountantsController : ControllerBase
    {
        public async Task<bool> SendVerificationSMSAsync(string phoneNumber, string code)
        {
            string ApiUrl = "http://gw.soft-line.az/sendsms";
            string Username = "softlineapi";
            string ApiKey = "pIZF18R4";
            string SenderName = "SOFTLINE";
            using (var client = new HttpClient())
            {
                try
                {
                    string text = $"Your verification code is: {code}";
                    string requestUrl = $"{ApiUrl}?user={Uri.EscapeDataString(Username)}" +
                                        $"&password={Uri.EscapeDataString(ApiKey)}" +
                                        $"&gsm={Uri.EscapeDataString(phoneNumber)}" +
                                        $"&from={Uri.EscapeDataString(SenderName)}" +
                                        $"&text={Uri.EscapeDataString(text)}";

                    System.Console.WriteLine(requestUrl);

                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    string responseContent = await response.Content.ReadAsStringAsync();

                    System.Console.WriteLine(responseContent);

                    var parts = responseContent.Split('&');
                    foreach (var part in parts)
                    {
                        var keyValue = part.Split('=');
                        System.Console.WriteLine(part);
                        if (keyValue.Length == 2 && keyValue[0] == "errno")
                        {
                            return keyValue[1] == "100";
                        }
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        private readonly IAccountantService _service;

        public AccountantsController(IAccountantService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("accountant", "admin")]
        // [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> sa()
        {
            return Ok(await SendVerificationSMSAsync("994556914560", "157 856"));
        }


        [HttpGet("[action]")]
        [SwaggerMultiGroup("accountant", "admin")]
        // [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> AppointmentsSalesRecord(Guid? expertId, DateTime? start, DateTime? end, int page = 1)
        {
            var response = await _service.AppointmentsSalesRecord(expertId, start, end, page);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("accountant", "admin")]
        // [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> ClinicSalesRecord(string? clinicKey, DateTime? start, DateTime? end, int page = 1)
        {
            var response = await _service.ClinicSalesRecord(clinicKey, start, end, page);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }


        [HttpGet("[action]")]
        [SwaggerMultiGroup("accountant", "admin")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> Payments(string? orderNumber, string? cupon, DateTime? startDate, DateTime? endDate, PaymentStatus? status, int page = 1, int limit = 10)
        {
            var response = await _service.Payments(orderNumber, cupon, startDate, endDate, status, page, limit);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("accountant", "admin")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> PaymentDetail(int id)
        {
            var response = await _service.PaymentDetail(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("accountant")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync(int page = 1, int limit = 10)
        {
            var response = await _service.GetAllAsync(page, limit);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _service.DeleteAsync(id);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAsync([FromBody] Create create)
        {
            var response = await _service.AddAsync(create);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}