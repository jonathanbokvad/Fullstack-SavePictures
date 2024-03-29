using ApiToDatabase.Models.DTOModels;
using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiToDatabase.Controllers;

[Route("api/folder")]
[ApiController]
[Authorize]
public class FoldersController : ControllerBase
{
    private readonly IFolderService _folderService;
    private readonly IJwtManager _jwtmanager;
    public FoldersController(IFolderService userService, IJwtManager jwtManager)
    {
        _folderService = userService;
        _jwtmanager = jwtManager;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Folder>>> GetFolders(string userId)
    {
        try
        {
            return Ok(await _folderService.GetFolders(userId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
    public async Task<ActionResult<List<Folder>>> UpdateFolderName(UpdateFolderRequest updateFolderRequest)
    {
        try
        {
            return Ok(await _folderService.UpdateFolderName(updateFolderRequest));

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
