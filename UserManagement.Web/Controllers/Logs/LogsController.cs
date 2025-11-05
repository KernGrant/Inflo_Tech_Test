using System.Linq;
using UserManagement.Services.Interfaces;

namespace UserManagement.WebMS.Controllers
{
    [Route("logs")]
    public class LogsController : Controller
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }
        
        [HttpGet("")]
        [HttpGet("log")]
        public IActionResult List()
        {
            var logs = _logService.GetAllLogs()
                                  .OrderByDescending(l => l.Timestamp)
                                  .ToList();

            return View(logs);
        }

        [HttpGet("details/{id:int}")]
        public IActionResult Details(int id)
        {
            var log = _logService.GetAllLogs().FirstOrDefault(l => l.Id == id);

            if (log == null)
            {
                return NotFound();
            }
                

            return View(log);
        }
    }
}
