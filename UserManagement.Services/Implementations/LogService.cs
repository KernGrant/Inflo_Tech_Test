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
            // Seed some initial logs
            for (int i = 1; i <= 50; i++)
            {
                _logs.Add(new UserActionLog
                {
                    Id = i,
                    UserId = (i % 10) + 1, // assign to users 1–10
                    Action = i % 2 == 0 ? "Create" : "Update",
                    Timestamp = DateTime.UtcNow.AddMinutes(-i),
                    Details = $"Example log entry {i} for user {(i % 10) + 1}."
                });
            }

            AddLog(new UserActionLog
            {
                UserId = 1,
                Action = "Create",
                Timestamp = DateTime.UtcNow.AddDays(-5),
                Details = "User John Doe was created."
            });

            AddLog(new UserActionLog
            {
                UserId = 2,
                Action = "Update",
                Timestamp = DateTime.UtcNow.AddDays(-2),
                Details = "Updated email for Jane Smith."
            });

            AddLog(new UserActionLog
            {
                UserId = 1,
                Action = "Delete",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                Details = null //Null check example - will show generic message "No additional information..."
            });
        }

        public IEnumerable<UserActionLog> GetAllLogs()
        {
            return _logs.OrderByDescending(l => l.Timestamp);
        }

        public IEnumerable<UserActionLog> GetLogsForSpecificUser(int userId)
        {
            return _logs.Where(l => l.UserId == userId).OrderByDescending(l => l.Timestamp);
        }

        public UserActionLog? GetLogById(int logId)
        {
            return _logs.FirstOrDefault(l => l.Id == logId);
        }

        public void AddLog(UserActionLog log)
        {
            log.Id = _logs.Any() ? _logs.Max(l => l.Id) + 1 : 1;            
            _logs.Add(log);
        }
    } 
}
