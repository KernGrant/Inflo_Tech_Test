using System.Collections.Generic;
using System.Linq;
using UserManagement.Services.Interfaces;
using UserManagement.Data.Entities;
using System;

namespace UserManagement.Services.Implementations
{
    public class LogService : ILogService
    {
        private readonly List<UserActionLog> _logs = new();

        public LogService()
        {
            _logs.AddRange(new[]
            {
                new UserActionLog { Id = 1, UserId = 1, Action = "Created user", Timestamp = DateTime.Now.AddMinutes(-30) },
                new UserActionLog { Id = 2, UserId = 2, Action = "Edited profile", Timestamp = DateTime.Now.AddMinutes(-10) },
                new UserActionLog { Id = 3, UserId = 1, Action = "Deleted account", Timestamp = DateTime.Now }
            });
        }

        public IEnumerable<UserActionLog> GetAllLogs() => _logs;

        public IEnumerable<UserActionLog> GetLogsForSpecificUser(int userId)
        {
            return _logs.Where(l => l.UserId == userId);
        }

        public UserActionLog? GetLogById(int logId)
        {
            return _logs.FirstOrDefault(l => l.Id == logId);
        }

        public void AddLog(UserActionLog log)
        {
            log.Id = _logs.Count + 1;
            _logs.Add(log);
        }
    } 
}
