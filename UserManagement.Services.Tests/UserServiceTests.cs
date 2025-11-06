using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Services.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IDataContext> _dataContextMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _dataContextMock = new Mock<IDataContext>();
            _userService = new UserService(_dataContextMock.Object);
        }

        private User CreateUser(int id = 1, string forename = "John", string surname = "Doe", string email = "john@example.com", bool isActive = true)
        {
            return new User
            {
                Id = id,
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = new DateOnly(2000, 1, 1)
            };
        }

        private List<User> CreateUsers(int count = 5)
        {
            var users = new List<User>();
            for (int i = 1; i <= count; i++)
            {
                users.Add(CreateUser(i, $"User{i}", $"Test{i}", $"user{i}@example.com"));
            }
            return users;
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = CreateUsers();
            _dataContextMock.Setup(d => d.GetAllAsync<User>()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingId_ReturnsUser()
        {
            // Arrange
            var user = CreateUser(1);
            _dataContextMock.Setup(d => d.GetAllAsync<User>()).ReturnsAsync(new List<User> { user });

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _dataContextMock.Setup(d => d.GetAllAsync<User>()).ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.GetUserByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddUserAsync_CallsCreateAsync()
        {
            // Arrange
            var user = CreateUser();
            _dataContextMock.Setup(d => d.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _userService.AddUserAsync(user);

            // Assert
            _dataContextMock.Verify(d => d.CreateAsync(It.Is<User>(u => u == user)), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_CallsUpdateAsync()
        {
            // Arrange
            var user = CreateUser();
            _dataContextMock.Setup(d => d.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _userService.UpdateUserAsync(user);

            // Assert
            _dataContextMock.Verify(d => d.UpdateAsync(It.Is<User>(u => u == user)), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_CallsDeleteAsync()
        {
            // Arrange
            var user = CreateUser();
            _dataContextMock.Setup(d => d.DeleteAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _userService.DeleteUserAsync(user);

            // Assert
            _dataContextMock.Verify(d => d.DeleteAsync(It.Is<User>(u => u == user)), Times.Once);
        }
    }
}
