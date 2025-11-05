using System;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILogService> _logService = new();
    private UsersController CreateController() => new(_userService.Object, _logService.Object);

    [Fact]
    public void Create_Get_ReturnsViewWithModel()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Create();

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<UserCreateViewModel>();
    }

    [Fact]
    public void Create_Post_ValidModel_RedirectsToList()
    {
        // Arrange
        var controller = CreateController();
        var model = new UserCreateViewModel
        {
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com",
            IsActive = true,
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act
        var result = controller.Create(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");

        _userService.Verify(s => s.AddUser(It.Is<User>(u =>
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email
        )), Times.Once);
    }

    [Fact]
    public void Create_Post_InvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var controller = CreateController();
        controller.ModelState.AddModelError("Forename", "Required");

        var model = new UserCreateViewModel();

        // Act
        var result = controller.Create(model);

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().Be(model);
    }

    [Fact]
    public void Edit_Get_ValidId_ReturnsViewWithModel()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Doe",
            Email = "john@example.com",
            IsActive = true,
            DateOfBirth = new DateOnly(2000, 01, 01)
        };

        _userService.Setup(s => s.GetUserById(1)).Returns(new[] { user });
        var controller = CreateController();

        // Act
        var result = controller.Edit(1);

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.Model.As<UserCreateViewModel>().Forename.Should().Be("John");
    }

    [Fact]
    public void Edit_Post_ValidModel_CallsUpdateAndRedirects()
    {
        // Arrange
        var user = new User { Id = 1,
                                Forename = "John",
                                Surname = "Smith",
                                Email = "jsmith@testing.com",
                                DateOfBirth = new DateOnly(2001, 01, 01)
        };

        _userService.Setup(s => s.GetUserById(1)).Returns(new[] { user });
        var controller = CreateController();

        var model = new UserCreateViewModel
        {
            Id = 1,
            Forename = "Updated",
            Surname = "User",
            Email = "updated@example.com",
            DateOfBirth = new DateOnly(2002, 02, 02),
            IsActive = true
        };

        // Act
        var result = controller.Edit(1, model);

        // Assert
        _userService.Verify(s => s.UpdateUser(It.Is<User>(u =>
            u.Forename == "Updated" &&
            u.Surname == "User" &&
            u.Email == "updated@example.com" &&
            u.DateOfBirth == new DateOnly(2002, 02, 02) &&
            u.IsActive == true
        )), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    [Fact]
    public void Delete_Get_ValidId_ReturnsViewWithModel()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "jsmith@testing.com",
            DateOfBirth = new DateOnly(2001, 01, 01)
        };

        _userService.Setup(s => s.GetUserById(1)).Returns(new[] { user });
        var controller = CreateController();

        // Act
        var result = controller.Delete(1);

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.Model.As<UserDetailsViewModel>().Forename.Should().Be("John");
    }

    [Fact]
    public void DeleteConfirmed_ValidId_CallsDeleteAndRedirects()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Smith",
            Email = "jsmith@testing.com",
            DateOfBirth = new DateOnly(2001, 01, 01)
        };

        _userService.Setup(s => s.GetUserById(1)).Returns(new[] { user });
        var controller = CreateController();

        // Act
        var result = controller.DeleteConfirmed(1);

        // Assert
        _userService.Verify(s => s.DeleteUser(user), Times.Once);
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }


}
