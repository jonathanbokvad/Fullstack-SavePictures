using ApiToDatabase.Models;
using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiToDatabase.Controllers;

[Route("api/folder")]
[ApiController]
public class FoldersController : ControllerBase
{
    private readonly IFolderService _folderService;
    //private readonly IMongoCollection<Folder> _folders;

    public FoldersController(IFolderService userService)
    {
        //  _folders = database.GetCollection<Folder>("folders");
        _folderService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Folder>>> GetFolders(string userId)
    {
        return Ok(await _folderService.GetFolders(userId));
    }

    [HttpPost]
    public async Task<ActionResult<List<Folder>>> CreateFolders(CreateFolderRequest createFolderRequest)
    {
        try
        {
            return Ok(await _folderService.CreateFolder(createFolderRequest));

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult<List<Folder>>> UpdateFolderName(FolderRequest folderRequest)
    {
        try
        {
            return Ok(await _folderService.UpdateFolderName(folderRequest));

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete]
    public async Task<ActionResult<List<Folder>>> DeleteFolder([FromBody] string folderId)
    {
        try
        {
            return Ok(await _folderService.DeleteFolder(folderId));

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
