using ApiToDatabase.Models;
using ApiToDatabase.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiToDatabase.Services;

public interface IUserService
{
    Task<bool> ValidateUserAsync(UserRequest userRequest);
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(string id);
    Task<bool> UserExist(string username);
    Task<User?> GetUserByNameAsync(string username);
    Task<User> CreateUserAsync(UserRequest userRequest);
    Task UpdateUserAsync(string id, User updatedUser);
    Task RemoveUserAsync(string id);

}

