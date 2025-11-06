using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Entities;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILogService> _logService = new();

    private UsersController CreateController() => new(_userService.Object, _logService.Object);

    #region Helpers

    private User DefaultUser(int id = 1,
                             string forename = "John",
                             string surname = "Smith",
                             string email = "jsmith@test.com",
                             bool isActive = true,
                             DateOnly? dob = null)
        => new()
        {
            Id = id,
            Forename = forename,
            Surname = surname,
            Email = email,
            IsActive = isActive,
            DateOfBirth = dob ?? new DateOnly(2000, 01, 01)
        };

    private void SetupUserService(IEnumerable<User>? users = null)
    {
        users ??= new List<User> { DefaultUser() };
        _userService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);
        foreach (var user in users)
        {
            _userService.Setup(s => s.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        }
    }

    #endregion

    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        var users = new[] { DefaultUser() };
        SetupUserService(users);
        var controller = CreateController();

        var result = await controller.List();

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    #region Create Tests
    [Fact]
    public void Create_Get_ReturnsViewWithModel()
    {
        var controller = CreateController();

        var result = controller.Create();

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<UserCreateViewModel>();
    }

    [Fact]
    public async Task Create_Post_ValidModel_RedirectsToList_AndLogsAction()
    {
        var controller = CreateController();
        var model = new UserCreateViewModel
        {
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com",
            IsActive = true,
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        _userService.Setup(s => s.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _logService.Setup(s => s.AddLogAsync(It.IsAny<UserActionLog>())).Returns(Task.CompletedTask);

        var result = await controller.Create(model);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");

        _userService.Verify(s => s.AddUserAsync(It.Is<User>(u =>
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email
        )), Times.Once);

        _logService.Verify(s => s.AddLogAsync(It.Is<UserActionLog>(l =>
            l.Action == "Create"
        )), Times.Once);
    }

    [Fact]
    public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Forename", "Required");
        var model = new UserCreateViewModel();

        var result = await controller.Create(model);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().Be(model);
    }
    #endregion

    #region Edit Tests
    [Fact]
    public async Task Edit_Get_ValidId_ReturnsViewWithModel()
    {
        var user = DefaultUser();
        SetupUserService(new[] { user });
        var controller = CreateController();

        var result = await controller.Edit(user.Id);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.As<UserCreateViewModel>().Forename.Should().Be(user.Forename);
    }

    [Fact]
    public async Task Edit_Get_InvalidId_ReturnsNotFound()
    {
        _userService.Setup(s => s.GetUserByIdAsync(999)).ReturnsAsync((User?)null);
        var controller = CreateController();

        var result = await controller.Edit(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Edit_Post_ValidModel_CallsUpdateAndRedirects_AndLogsAction()
    {
        var user = DefaultUser();
        SetupUserService(new[] { user });
        _userService.Setup(s => s.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _logService.Setup(s => s.AddLogAsync(It.IsAny<UserActionLog>())).Returns(Task.CompletedTask);
        var controller = CreateController();

        var model = new UserCreateViewModel
        {
            Id = user.Id,
            Forename = "Updated",
            Surname = "User",
            Email = "updated@example.com",
            DateOfBirth = new DateOnly(2002, 02, 02),
            IsActive = true
        };

        var result = await controller.Edit(user.Id, model);

        _userService.Verify(s => s.UpdateUserAsync(It.Is<User>(u =>
            u.Forename == model.Forename &&
            u.Surname == model.Surname &&
            u.Email == model.Email &&
            u.DateOfBirth == model.DateOfBirth &&
            u.IsActive == model.IsActive
        )), Times.Once);

        _logService.Verify(s => s.AddLogAsync(It.Is<UserActionLog>(l =>
            l.Action == "Updated" &&
            l.UserId == user.Id
        )), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }
    #endregion

    #region Delete Tests
    [Fact]
    public async Task Delete_Get_ValidId_ReturnsViewWithModel()
    {
        var user = DefaultUser();
        SetupUserService(new[] { user });
        var controller = CreateController();

        var result = await controller.Delete(user.Id);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.As<UserDetailsViewModel>().Forename.Should().Be(user.Forename);
    }

    [Fact]
    public async Task Delete_Get_InvalidId_ReturnsNotFound()
    {
        _userService.Setup(s => s.GetUserByIdAsync(999)).ReturnsAsync((User?)null);
        var controller = CreateController();

        var result = await controller.Delete(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteConfirmed_ValidId_CallsDeleteAndRedirects_AndLogsAction()
    {
        var user = DefaultUser();
        SetupUserService(new[] { user });
        _userService.Setup(s => s.DeleteUserAsync(user)).Returns(Task.CompletedTask);
        _logService.Setup(s => s.AddLogAsync(It.IsAny<UserActionLog>())).Returns(Task.CompletedTask);
        var controller = CreateController();

        var result = await controller.DeleteConfirmed(user.Id);

        _userService.Verify(s => s.DeleteUserAsync(user), Times.Once);
        _logService.Verify(s => s.AddLogAsync(It.Is<UserActionLog>(l =>
            l.Action == "Deleted" &&
            l.UserId == user.Id
        )), Times.Once);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }
    #endregion
}
