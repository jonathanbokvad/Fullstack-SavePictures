using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiToDatabase.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

 
    public UserController(IUserService userService)
    {
        _userService = userService; 
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
        if (_userService.ValidateUserAsync(user).Result)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                Issuer = System.Configuration["Jwt:key"].ToString(),                
            }
            JwtSecurityTokenHandler tokenHandler = new();
            

           return Ok(token);
        }
        return BadRequest();
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