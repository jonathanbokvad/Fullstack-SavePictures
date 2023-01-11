using ApiToDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiToDatabase.Services;

public interface IUserService
{
    Task<bool> ValidateUserAsync(User user);
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string id);
    Task<bool> UserExist(string username);
    Task<User?> GetUserByNameAsync(string username);
    Task CreateUserAsync(User newUser);
    Task UpdateUserAsync(string id, User updatedUser);
    Task RemoveUserAsync(string id);

    
    Task<List<Folder>> GetFolders();
}

