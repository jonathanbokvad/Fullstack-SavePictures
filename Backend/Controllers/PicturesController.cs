using ApiToDatabase.Models;
using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiToDatabase.Controllers
{
    [Route("api/pictures")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureService _pictureService;
        //private readonly IMongoCollection<Folder> _folders;

        public PicturesController(IPictureService pictureService)
        {
            //  _folders = database.GetCollection<Folder>("folders");
            _pictureService = pictureService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Picture>>> GetPicturesFromFolder(string folderId)
        {
            try
            {
                return Ok(await _pictureService.GetPictures(folderId));
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
                var ds = await _pictureService.DeletePicture(pictureId);
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
                var result = _pictureService.CreatePicture(pictureRequest);
                return Created("/api/pictures", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
