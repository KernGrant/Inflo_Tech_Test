using System;
using System.Linq;
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

}
