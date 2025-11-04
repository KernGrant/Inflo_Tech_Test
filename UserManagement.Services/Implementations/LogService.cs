using System.Collections.Generic;
using System.Linq;
using UserManagement.Services.Interfaces;
using UserManagement.Services.Models.Logging;

namespace UserManagement.Services.Implementations
{
    public class LogService : ILogService
    {
        private readonly List<UserActionLog> _logs = new();

        public void AddLog(UserActionLog log) => _logs.Add(log);

        public IEnumerable<UserActionLog> GetLogs() => _logs;

        public IEnumerable<UserActionLog> GetLogsForUser(int userId) =>
            _logs.Where(l => l.UserId == userId);
    }
}
