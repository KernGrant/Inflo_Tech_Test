using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Tests;

public class LogServiceTests
{
    private readonly Mock<IDataContext> _dataContextMock = new();
    private readonly ILogService _logService;

    public LogServiceTests()
    {
        _logService = new LogService(_dataContextMock.Object);
    }

    #region Helpers

    private UserActionLog SetupLog(int id = 1,
                                   int userId = 1,
                                   string action = "Test Action",
                                   DateTime? timestamp = null,
                                   string? details = "Some details")
        => new UserActionLog
        {
            Id = id,
            UserId = userId,
            Action = action,
            Timestamp = timestamp ?? DateTime.UtcNow,
            Details = details
        };

    private void SetupDataContextWithLogs(IEnumerable<UserActionLog> logs)
    {
        _dataContextMock.Setup(d => d.GetAllAsync<UserActionLog>())
                        .ReturnsAsync(logs.ToList());
    }

    #endregion

    [Fact]
    public async Task GetAllLogsAsync_ReturnsAllLogs()
    {
        var logs = new List<UserActionLog>
        {
            SetupLog(1),
            SetupLog(2)
        };
        SetupDataContextWithLogs(logs);

        var result = await _logService.GetAllLogsAsync();

        result.Should().HaveCount(2)
            .And.BeEquivalentTo(logs);
    }

    [Fact]
    public async Task GetLogByIdAsync_ExistingId_ReturnsLog()
    {
        var log = SetupLog(1);
        SetupDataContextWithLogs(new[] { log });

        var result = await _logService.GetLogByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetLogByIdAsync_NonExistingId_ReturnsNull()
    {
        SetupDataContextWithLogs(Array.Empty<UserActionLog>());

        var result = await _logService.GetLogByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddLogAsync_CallsCreateAsync()
    {
        var log = SetupLog(1);

        await _logService.AddLogAsync(log);

        _dataContextMock.Verify(d => d.CreateAsync(log), Times.Once);
    }

    [Fact]
    public async Task GetLogsForSpecificUserAsync_ReturnsOnlyThatUsersLogs()
    {
        var logs = new List<UserActionLog>
        {
            SetupLog(1, userId: 1),
            SetupLog(2, userId: 2),
            SetupLog(3, userId: 1)
        };
        SetupDataContextWithLogs(logs);

        var result = await _logService.GetLogsForSpecificUserAsync(1);

        result.Should().OnlyContain(l => l.UserId == 1)
              .And.HaveCount(2);
    }
}
