using Core.DataAccess;
using Core.Helpers;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Helpers;
using Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Swagger;

namespace WebApi.Controllers.AdminLevel
{
    [ApiController]
    [SwaggerMultiGroup("admin")]
    [Route("nam/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IRepositoryBase<Log, Log, AppDbContext> _log;

        public LogController(IRepositoryBase<Log, Log, AppDbContext> log)
        {
            _log = log;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Logs(LogFilter filter, int page = 1, int limit = 50)
        {
            using (var context = new AppDbContext())
            {
                IQueryable<Log> query = context.Logs.OrderByDescending(c => c.Date);

                DtoFilter<Log, Log> dtoFilter = new()
                {
                    Limit = limit,
                    Page = page,
                    Selector = c => c
                };
                switch (filter)
                {
                    case LogFilter.Success:
                        query = query.Where(log => log.IsSucces);
                        break;
                    case LogFilter.Fail:
                        query = query.Where(log => !log.IsSucces);
                        break;
                    case LogFilter.All:
                    default:
                        break;
                }

                return Ok(await _log.GetAllAsync(query, dtoFilter));
            }
        }
    }
}