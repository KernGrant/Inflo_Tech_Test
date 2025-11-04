using System.Collections.Generic;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Interfaces
{
    public interface ILogService
    {
        IEnumerable<UserActionLog> GetAllLogs();
        IEnumerable<UserActionLog> GetLogsForSpecificUser(int userId);
        UserActionLog? GetLogById(int logId);
        void AddLog(UserActionLog log);               
    }

}
