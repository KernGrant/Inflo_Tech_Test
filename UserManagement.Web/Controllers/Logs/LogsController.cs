using System;
using System.Linq;
using UserManagement.Services.Interfaces;

namespace UserManagement.WebMS.Controllers
{
    [Route("logs")]
    public class LogsController : Controller
    {
        private readonly ILogService _logService;
        private const int PageSize = 20; // entries per page

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }
        
        [HttpGet("")]
        [HttpGet("list")]
        public IActionResult List(int page = 1, int? userId = null)
        {
            var logsQuery = _logService.GetAllLogs()
                                        .AsQueryable();


            if (userId.HasValue)
            {
                logsQuery = logsQuery.Where(l => l.UserId == userId); //If userId is provided, filter logs by that user
            }

            var totalLogs = logsQuery.Count();

            var totalPages = (int)Math.Ceiling(totalLogs / (double)PageSize); //Calculate total pages needed

            var logs = logsQuery
                        .OrderByDescending(l => l.Timestamp)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["FilteredUserId"] = userId;

            return View(logs);
        }

        [HttpGet("details/{id:int}")]
        public IActionResult Details(int id, int page = 1, int? userId = null)
        {
            var log = _logService.GetAllLogs()                                    
                                    .FirstOrDefault(l => l.Id == id);

            if (log == null)
            {
                return NotFound();
            }

            ViewData["CurrentPage"] = page;
            ViewData["FilteredUserId"] = userId;

            return View(log);
        }
    }
}
