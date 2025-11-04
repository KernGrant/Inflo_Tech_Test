using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return all users
    /// </summary>    
    /// <returns></returns>
    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public IEnumerable<User> GetUserById(int id)
    {
        var user = _dataAccess.GetAll<User>().FirstOrDefault(u => u.Id == id);

        if (user != null)
        {
            return new List<User> { user };
        }
        else
        {
            return Enumerable.Empty<User>();
        }
    }

    public void AddUser(User user)
    {
        _dataAccess.Create(user);
    }

    public void UpdateUser(User user)
    {
        _dataAccess.Update(user);
    }

    public void DeleteUser(User user)
    {
        _dataAccess.Delete(user);
    }
}
