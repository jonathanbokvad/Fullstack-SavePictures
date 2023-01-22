using ApiToDatabase.Models;
using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ApiToDatabase.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase 
{
    private readonly IUserService _userService;
    private readonly IJwtManager _jwtManager;
    public UserController(IUserService userService, IJwtManager jwtManager)
    {
        _userService = userService;
        _jwtManager = jwtManager;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> GetUser(string id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(UserRequest userRequest)
    {
        try
        {
            //_jwtManager.ValidateToken(HttpContext.Request.Headers.Authorization.ToString().Split("Bearer ")[1]);
            if (_userService.ValidateUserAsync(userRequest).Result)
            {
                return Ok(new string[] { _jwtManager.CreateToken(), _userService.GetUserByNameAsync(userRequest.UserName).Result.Id } );
            }
            return Unauthorized();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    [Route("createacc")]
    public async Task<IActionResult> CreateUser(UserRequest userRequest)
    {
        //var req = HttpContext.Request.ReadFormAsync().Result;
        if (userRequest is not null && !_userService.UserExist(userRequest.UserName).Result)
        {
            var newUser = await _userService.CreateUserAsync(userRequest);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
        return BadRequest();
    }


    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateUser(string id, User updatedUser)
    //{
    //    var user = await _userService.GetUserAsync(id);
    //    if (user is null)
    //    {
    //        return NotFound();
    //    }

    //    updatedUser.Id = user.Id;

    //    await _userService.UpdateUserAsync(id, updatedUser);
    //    return NoContent();
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> RemoveUser(string id)
    //{
    //    var user = await _userService.GetUserAsync(id);
    //    if (user is null)
    //        return NotFound();

    //    await _userService.RemoveUserAsync(id);
    //    return NoContent();
    //}
}