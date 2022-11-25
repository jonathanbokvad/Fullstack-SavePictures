using ApiToDatabase.Models;

namespace ApiToDatabase.Services;

public interface IUserService
{
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string id);
    Task CreateUserAsync(User newUser);
    Task UpdateUserAsync(string id, User updatedUser);
    Task RemoveUserAsync(string id);
}

