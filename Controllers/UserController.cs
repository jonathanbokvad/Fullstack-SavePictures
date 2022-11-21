using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Mvc;
namespace ApiToDatabase.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService; 
    }

    [HttpGet]
    public async Task<List<User>> Get()
    => await _userService.GetUsersAsync();

    [HttpPost("userinfo")]
    public void SendToDatabase()
    {
        
    }
}