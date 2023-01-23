using ApiToDatabase.Models.DTOModels;
using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ApiToDatabase.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtManager _jwtManager;
    public UserController(IUserService userService, IJwtManager jwtManager)
    {
        _userService = userService;
        _jwtManager = jwtManager;
    }

    [HttpGet]
    public async Task<ActionResult<User?>> GetUser(string userId)
    {
        try
        {
            var user = await _userService.GetUserById(userId);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LogIn(UserRequest userRequest)
    {
        try
        {
            if (_userService.ValidateUser(userRequest).Result)
            {
                return Ok(new string[] { _jwtManager.CreateToken(), _userService.GetUserByName(userRequest.UserName).Result.Id });
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
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(UserRequest userRequest)
    {
        try
        {
            if (userRequest is not null && !_userService.UserExist(userRequest.UserName).Result)
            {
                var newUser = await _userService.CreateUser(userRequest);
                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            return BadRequest("Empty request or user already exists!");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {

            var user = await _userService.GetUserById(userId);
            if (user is null)
            {
                return NotFound();
            }

            await _userService.DeleteUser(userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}