using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> CreateUser(User newUser)
    {
        //var req = HttpContext.Request.ReadFormAsync().Result;
        if (newUser is not null)
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