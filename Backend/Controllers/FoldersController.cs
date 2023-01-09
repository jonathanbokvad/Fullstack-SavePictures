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

[Route("api/[controller]")]
[ApiController]
public class FoldersController : ControllerBase
{
  private readonly IMongoCollection<Folder> _folders;

  public FoldersController(IMongoDatabase database)
  {
    _folders = database.GetCollection<Folder>("folders");
  }

  [HttpGet]
  public async Task<ActionResult<List<Folder>>> GetFolders()
  {
    var pipeline = new[] {
      new BsonDocument("$lookup", new BsonDocument
      {
        { "from", "pictures" },
        { "localField", "pictures" },
        { "foreignField", "_id" },
        { "as", "pictures" }
      })
    };

    var result = await _folders.AggregateAsync<Folder>(pipeline);
    return result.ToList();
  }
}
