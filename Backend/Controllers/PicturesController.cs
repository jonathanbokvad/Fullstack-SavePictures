using ApiToDatabase.Models;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            try
            {
                return Ok(await _userService.GetPictures(folderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePicture(string pictureId) 
        {
            try
            {
                var ds = await _userService.DeletePicture(pictureId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePicture([FromBody] PictureRequest pictureRequest)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(pictureRequest.Data);
                var result = _userService.CreatePictureTest(imageBytes, "");
                return Created("https://localhost:7019/api/pictures", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

public class PictureRequest
{
    public string Data { get; set; }
}