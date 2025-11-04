using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet("list")]
    public ViewResult List(bool? isActive = null)
    {
        var items = _userService.GetAll()
            .Where(p => !isActive.HasValue || p.IsActive == isActive)
            .Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("view/{id:int}")]
    public IActionResult View(int? id = null)
    {        
        if(!id.HasValue)
        {
            throw new ArgumentNullException("A user ID was not provided or could not be found.");            
        }

        // Retrieve the user based on the id
        var user = _userService.GetUserById((int)id).FirstOrDefault();
        
        if (user == null)
        {
            return NotFound();
        }

        //Map to viewmodel
        var userDetails = new UserDetailsViewModel
        {
            Id = (int)user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };

        return View(userDetails);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new UserCreateViewModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(UserCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }


        var newUser = new User
        {
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth ?? DateOnly.MinValue
        };
        
        _userService.AddUser(newUser);

        // Redirect back to the list
        return RedirectToAction("List");
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int? id)
    {
        if (!id.HasValue)
            return BadRequest("No user ID provided.");

        var user = _userService.GetUserById(id.Value).FirstOrDefault();

        if (user == null)
            return NotFound();

        // Map domain model to ViewModel
        var model = new UserCreateViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, UserCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _userService.GetUserById(id).FirstOrDefault();
        if (user == null)
            return NotFound();

        // Update domain model
        user.Id = model.Id;
        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.Email = model.Email;
        user.DateOfBirth = model.DateOfBirth;
        user.IsActive = model.IsActive;
        
        _userService.UpdateUser(user);

        return RedirectToAction("List");
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int? id)
    {
        if (!id.HasValue)
            return BadRequest("A user ID must be provided.");

        var user = _userService.GetUserById(id.Value).FirstOrDefault();
        if (user == null)
            return NotFound();

        var model = new UserDetailsViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };

        return View(model); // confirm deletion
    }
    
    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _userService.GetUserById(id).FirstOrDefault();
        if (user == null)
            return NotFound();

        _userService.DeleteUser(user);

        return RedirectToAction("List");
    }


}
