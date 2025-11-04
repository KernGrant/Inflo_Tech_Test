using System.Collections.Generic;
using UserManagement.Services.Models.Logging;

namespace UserManagement.Services.Interfaces
{
    public interface ILogService
    {
        void AddLog(UserActionLog log);
        IEnumerable<UserActionLog> GetLogs();
        IEnumerable<UserActionLog> GetLogsForUser(int userId);
    }

}
