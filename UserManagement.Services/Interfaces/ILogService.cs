using System.Collections.Generic;
using UserManagement.Data.Entities;
using System.Threading.Tasks;

namespace UserManagement.Services.Interfaces
{
    public interface ILogService
    {
        Task<IEnumerable<UserActionLog>> GetAllLogsAsync();
        Task<IEnumerable<UserActionLog>> GetLogsForSpecificUserAsync(int userId);        
        Task<UserActionLog?> GetLogByIdAsync(int logId);
        Task AddLogAsync(UserActionLog log);               
    }

}
