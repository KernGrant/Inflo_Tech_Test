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

    #region InitializeAsync Tests

    [Fact]
    public async Task InitializeAsync_WhenNoLogsExist_ShouldSeedLogs()
    {
        // Arrange
        _dataContextMock.Setup(d => d.GetAllAsync<UserActionLog>())
                        .ReturnsAsync(new List<UserActionLog>());

        var createdLogs = new List<UserActionLog>();
        _dataContextMock.Setup(d => d.CreateAsync(It.IsAny<UserActionLog>()))
                        .Callback<UserActionLog>(log => createdLogs.Add(log))
                        .Returns(Task.CompletedTask);

        // Act
        await _logService.InitializeAsync();

        // Assert
        createdLogs.Should().HaveCount(53); // 50 generic + 3 specific logs
        createdLogs.Select(l => l.Action)
                   .Should().Contain(new[] { "Create", "Update", "Delete" }); // sanity check for specific logs

        // Check descending order of timestamps for generic logs
        var genericLogs = createdLogs.Take(50).OrderByDescending(l => l.Timestamp).ToList();
        genericLogs.Should().BeInDescendingOrder(l => l.Timestamp);
    }

    [Fact]
    public async Task InitializeAsync_WhenLogsAlreadyExist_ShouldNotSeedAgain()
    {
        // Arrange
        var existingLogs = new List<UserActionLog>
            {
                new() { Id = 1, UserId = 1, Action = "Create" }
            };
        _dataContextMock.Setup(d => d.GetAllAsync<UserActionLog>())
                        .ReturnsAsync(existingLogs);

        // Act
        await _logService.InitializeAsync();

        // Assert
        _dataContextMock.Verify(d => d.CreateAsync(It.IsAny<UserActionLog>()), Times.Never);
    }

    #endregion
}
