using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace ApiToDatabase.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMongoDbServices _userService;
    private readonly IJwtManager _jwtManager;
    public UserController(IMongoDbServices userService, IJwtManager jwtManager)
    {
        _userService = userService;
        _jwtManager = jwtManager;
    }

    [HttpGet]
    public async Task<List<User>> GetUsers()
    => await _userService.GetUsersAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> GetUser(string id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user is null)
            return NotFound();

        return Ok(user);
    }
    [HttpPost]
    public async Task<IActionResult> LogIn(User user)
    {
        try
        {
            //_jwtManager.ValidateToken(HttpContext.Request.Headers.Authorization.ToString().Split("Bearer ")[1]);
            if (_userService.ValidateUserAsync(user).Result)
            {
                return Ok(_jwtManager.CreateToken());
            }
            return Unauthorized();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    [Route("CreateAcc")]
    public async Task<IActionResult> CreateUser(User newUser)
    {
        //var req = HttpContext.Request.ReadFormAsync().Result;
        if (newUser is not null && !_userService.UserExist(newUser.UserName).Result)
        {
            await _userService.CreateUserAsync(newUser);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
        return BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, User updatedUser)
    {
        var user = await _userService.GetUserAsync(id);
        if (user is null)
            return NotFound();

        updatedUser.Id = user.Id;

        await _userService.UpdateUserAsync(id, updatedUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveUser(string id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user is null)
            return NotFound();

        await _userService.RemoveUserAsync(id);
        return NoContent();
    }
}