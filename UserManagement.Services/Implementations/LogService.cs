using System.Collections.Generic;
using System.Linq;
using UserManagement.Services.Interfaces;
using UserManagement.Data.Entities;
using System;
using System.Threading.Tasks;

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

            AddLogAsync(new UserActionLog
            {
                UserId = 1,
                Action = "Create",
                Timestamp = DateTime.UtcNow.AddDays(-5),
                Details = "User John Doe was created."
            });

            AddLogAsync(new UserActionLog
            {
                UserId = 2,
                Action = "Update",
                Timestamp = DateTime.UtcNow.AddDays(-2),
                Details = "Updated email for Jane Smith."
            });

            AddLogAsync(new UserActionLog
            {
                UserId = 1,
                Action = "Delete",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                Details = null //Null check example - will show generic message "No additional information..."
            });
        }

        public Task<IEnumerable<UserActionLog>> GetAllLogsAsync()
        {
            return Task.FromResult(_logs.OrderByDescending(l => l.Timestamp).AsEnumerable());            
        }

        public Task<IEnumerable<UserActionLog>> GetLogsForSpecificUserAsync(int userId)
        {
            return Task.FromResult(_logs.Where(l => l.UserId == userId).OrderByDescending(l => l.Timestamp).AsEnumerable());            
        }

        public UserActionLog? GetLogById(int logId)
        {
            return _logs.FirstOrDefault(l => l.Id == logId);
        }

        public Task AddLogAsync(UserActionLog log)
        {
            log.Id = _logs.Any() ? _logs.Max(l => l.Id) + 1 : 1;  //If no logs exist, start IDs at 1, otherwise add to the max existing ID          
            _logs.Add(log);
            return Task.CompletedTask;
        }
    } 
}
