using Buisness.Abstract;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.BasketDtos.BodyDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _service;
        public BasketsController(IBasketService service)
        {
            _service = service;
        }

        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        [HttpGet("[action]")]
        public async Task<IActionResult> MyBasket(string? cupon)
        {
            var userId = ControllerService.GetUserId(HttpContext);
            var response = await _service.MyBasket(userId, cupon);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddItem([FromBody] AddItem item)
        {
            item.UserId = ControllerService.GetUserId(HttpContext);
            var response = await _service.AddItem(item);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        } 

        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveAll()
        {
            var userId = ControllerService.GetUserId(HttpContext);
            var response = await _service.RemoveAll(userId);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }


        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            var userId = ControllerService.GetUserId(HttpContext);
            var response = await _service.RemoveItem(userId, itemId);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }

        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        [HttpGet("[action]")]
        public async Task<IActionResult> ItemCount()
        {
            var userId = ControllerService.GetUserId(HttpContext);

            using (var context = new AppDbContext())
            {
                var user = await context.Users
                    .Include(c => c.Basket)
                        .ThenInclude(c => c.Items)
                    .FirstOrDefaultAsync(c => c.Id == userId);
                if (user is null)
                {
                    return Unauthorized();
                }
                var cnt = user.Basket.Items.Sum(c => c.Count);
                var response = new SuccessDataResult<int>(data: cnt);
                return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
            }
        }

        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCount([FromBody] IEnumerable<UpdateRange> updates)
        {
            var userId = ControllerService.GetUserId(HttpContext);
            var response = await _service.UpdateCount(userId, updates);
            return new ObjectResult(response) { Value = response, StatusCode = (int)response.StatusCode };
        }
    }
}