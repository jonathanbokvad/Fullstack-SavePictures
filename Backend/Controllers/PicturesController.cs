using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiToDatabase.Controllers
{
    [Route("api/pictures")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IMongoDbServices _userService;
        //private readonly IMongoCollection<Folder> _folders;

        public PicturesController(IMongoDbServices userService)
        {
            //  _folders = database.GetCollection<Folder>("folders");
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Picture>>> GetPictures(string folderId)
        {
            return Ok(await _userService.GetPictures(folderId));
        }
        [HttpDelete]
        public IActionResult DeletePictures(string folderId) 
        {
            try
            {
                _userService.DeletePicture(folderId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }    
        }
    }
}
