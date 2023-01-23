using ApiToDatabase.Models.DTOModels;
using ApiToDatabase.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiToDatabase.Services;

public interface IUserService
{
    Task<bool> ValidateUser(UserRequest userRequest);
    Task<User> GetUserById(string id);
    Task<bool> UserExist(string username);
    Task<User> GetUserByName(string username);
    Task<User> CreateUser(UserRequest userRequest);
    Task DeleteUser(string id);
}

