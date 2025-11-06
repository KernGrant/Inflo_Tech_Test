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


        // Clear existing logs
        if (existingLogs.Any())
        {           
            await _dataContext.DeleteAllAsync(existingLogs);
        }

        var logs = new List<UserActionLog>();
        var random = new Random();

        // --- Seed 10 users with logical action sequences ---
        for (int userId = 1; userId <= 10; userId++)
        {
            // One "Create" per user (earliest)
            logs.Add(new UserActionLog
            {
                UserId = userId,
                Action = "Create",
                Timestamp = DateTime.UtcNow.AddDays(-random.Next(30, 60)),
                Details = $"User {userId} account created."
            });

            // Then random follow-up actions (Update/Delete)
            var actionCount = random.Next(3, 6);
            for (int i = 0; i < actionCount; i++)
            {
                var action = random.NextDouble() > 0.8 ? "Delete" : "Update";
                logs.Add(new UserActionLog
                {
                    UserId = userId,
                    Action = action,
                    Timestamp = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                    Details = $"User {userId} {action.ToLower()}d profile details."
                });
            }
        }
        
        logs.AddRange(new[]
        {
            new UserActionLog
            {
                UserId = 1,
                Action = "Create",
                Timestamp = DateTime.UtcNow.AddDays(-1),
                Details = "New user 'Alice Johnson' created."
            },
            new UserActionLog
            {
                UserId = 2,
                Action = "Update",
                Timestamp = DateTime.UtcNow.AddHours(-6),
                Details = "Updated email for Jane Smith. This entry is over 50 characters... " +
                                        "But you'll see the whole message when you click into it!" //Demonstrates longer details cutoff
            },
            new UserActionLog
            {
                UserId = 3,
                Action = "Delete",
                Timestamp = DateTime.UtcNow.AddHours(-2),
                Details = null                              //Demonstrates null details generic message 
            }
        });
        
        var totalLogs = logs.OrderByDescending(l => l.Timestamp).Take(50).ToList();

        foreach (var log in totalLogs)
            await _dataContext.CreateAsync(log);
    }
}
