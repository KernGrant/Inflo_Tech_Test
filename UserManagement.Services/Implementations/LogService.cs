using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

public class LogService : ILogService
{
    private readonly IDataContext _dataContext;

    public LogService(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<UserActionLog>> GetAllLogsAsync()
    {
        return (await _dataContext.GetAllAsync<UserActionLog>())
               .OrderByDescending(l => l.Timestamp);
    }

    public async Task<IEnumerable<UserActionLog>> GetLogsForSpecificUserAsync(int userId)
    {
        return (await _dataContext.GetAllAsync<UserActionLog>())
               .Where(l => l.UserId == userId)
               .OrderByDescending(l => l.Timestamp);
    }

    public async Task<UserActionLog?> GetLogByIdAsync(int logId)
    {
        return (await _dataContext.GetAllAsync<UserActionLog>())
               .FirstOrDefault(l => l.Id == logId);
    }

    public Task AddLogAsync(UserActionLog log)
    {
        return _dataContext.CreateAsync(log);
    }

    public async Task InitializeAsync()
    {
        var existingLogs = await _dataContext.GetAllAsync<UserActionLog>();

        // Only seed if none of the first example logs exist
        if (!existingLogs.Any(l => l.Action == "Create" || l.Action == "Update"))
        {
            var logsToSeed = new List<UserActionLog>();

            // 50 example logs
            for (int i = 0; i < 50; i++)
            {
                logsToSeed.Add(new UserActionLog
                {
                    UserId = (i % 10) + 1,
                    Action = i % 2 == 0 ? "Create" : "Update",
                    Timestamp = DateTime.UtcNow.AddMinutes(-i),
                    Details = $"Example log entry {i + 1} for user {(i % 10) + 1}."
                });
            }

            // 3 specific logs
            logsToSeed.AddRange(new[]
            {
            new UserActionLog
            {
                UserId = 1,
                Action = "Create",
                Timestamp = DateTime.UtcNow.AddDays(-5),
                Details = "User John Doe was created."
            },
            new UserActionLog
            {
                UserId = 2,
                Action = "Update",
                Timestamp = DateTime.UtcNow.AddDays(-2),
                Details = "Updated email for Jane Smith."
            },
            new UserActionLog
            {
                UserId = 1,
                Action = "Delete",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                Details = null
            }
        });

            foreach (var log in logsToSeed)
            {
                await _dataContext.CreateAsync(log);
            }
        }
    }

}
