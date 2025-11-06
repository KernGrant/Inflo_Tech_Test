using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;

    public UserService(IDataContext dataContext) => _dataAccess = dataContext;

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _dataAccess.GetAllAsync<User>();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var users = await _dataAccess.GetAllAsync<User>();
        return users.FirstOrDefault(u => u.Id == id);
    }

    public async Task AddUserAsync(User user)
    {
        await _dataAccess.CreateAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _dataAccess.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(User user)
    {
        await _dataAccess.DeleteAsync(user);
    }
}
