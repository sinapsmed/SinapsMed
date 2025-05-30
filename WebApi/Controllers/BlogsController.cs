using Buisness.Abstract;
using Entities.DTOs.BlogDtos;
using Entities.DTOs.BlogDtos.Update;
using Entities.DTOs.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Swagger;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("nam/[controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _service;
        public BlogsController(IBlogService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("admin", "expert")]
        public async Task<IActionResult> GetAll(int page = 1, int limit = 10)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            Guid.TryParse(User.Identity.Name, out var requesterId);
            var req = new ReqFrom { RequesterId = requesterId, Superiority = periority };
            var response = await _service.GetAll(page, limit, req);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        [Authorize(Roles = "Admin,Expert")]
        public async Task<IActionResult> GetDetailedAll(Guid? categoryId, int page = 1, int limit = 10)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            Guid.TryParse(User.Identity.Name, out var requesterId);
            var req = new ReqFrom { RequesterId = requesterId, Superiority = periority };
            var response = await _service.GetAll(page, limit, categoryId, req);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetFilterAll(Guid id, int page = 1, int limit = 10)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            Guid.TryParse(User.Identity.Name, out var requesterId);
            var req = new ReqFrom { RequesterId = requesterId, Superiority = periority };
            var response = await _service.GetAll(id, page, limit, req);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        [Authorize(Roles = "Expert,Admin")]
        public async Task<IActionResult> Create([FromBody] Create create)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            var username = User.Identity.Name;
            var response = await _service.Create(create, username is null ? null : Guid.Parse(username), periority);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        [Authorize(Roles = "Expert,Admin")]
        public async Task<IActionResult> UpdateData(Guid bolgId)
        {
            var response = await _service.BlogUpdateData(bolgId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPut("[action]")]
        [SwaggerMultiGroup("expert", "admin")]
        [Authorize(Roles = "Expert,Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateData updateData)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            Guid.TryParse(User.Identity.Name, out var requesterId);
            var req = new ReqFrom { RequesterId = requesterId, Superiority = periority };
            updateData.Updater = requesterId;
            var response = await _service.Update(updateData, req);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var response = await _service.DeleteCategory(categoryId);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("admin")]
        public async Task<IActionResult> CreateCategory([FromBody] List<CreateCategory> createCategories)
        {
            var response = await _service.CreateCategory(createCategories);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpDelete("[action]")]
        [SwaggerMultiGroup("admin")]
        [Authorize(Roles = "Admin,Expert")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            Guid.TryParse(User.Identity.Name, out var requesterId);
            var req = new ReqFrom { RequesterId = requesterId, Superiority = periority };
            var response = await _service.Delete(id, req);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }
        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _service.GetCategories();
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetCommnetReplies(Guid id, List<Guid> ids, int page = 1, int limit = 10)
        {
            page = page <= 0 ? 1 : page;
            var response = await _service.CommentReply(id, ids, page, limit);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> CategoryName(Guid id)
        {
            var response = await _service.CategoryName(id);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpGet("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var periority = ControllerService.GetSuperiority(HttpContext);
            Guid.TryParse(User.Identity.Name, out var requesterId);
            var req = new ReqFrom { RequesterId = requesterId, Superiority = periority };
            var response = await _service.GetById(id, req);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> AddComment([FromBody] AddComment comment)
        {
            comment.UserName = User.Identity.Name;
            var response = await _service.AddComment(comment);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> AddReplyComment([FromBody] AddComment comment)
        {
            comment.UserName = User.Identity.Name;
            var response = await _service.AddReplyComment(comment);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }

        [HttpPost("[action]")]
        [SwaggerMultiGroup("public")]
        public async Task<IActionResult> GetComments(Guid id, List<Guid> ids, int page = 1, int limit = 10)
        {
            var response = await _service.GetComments(id, ids, page, limit);
            return new ObjectResult(response) { StatusCode = (int)response.StatusCode, Value = response };
        }
    }
}