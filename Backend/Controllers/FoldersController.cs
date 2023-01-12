using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiToDatabase.Controllers;

[Route("api/folder")]
[ApiController]
public class FoldersController : ControllerBase
{
    private readonly IMongoDbServices _userService;
    //private readonly IMongoCollection<Folder> _folders;

    public FoldersController(IMongoDbServices userService)
    {
        //  _folders = database.GetCollection<Folder>("folders");
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Folder>>> GetFolders()
    {
        return Ok(await _userService.GetFolders());
    }
}
