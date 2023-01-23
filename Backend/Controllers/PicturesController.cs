using ApiToDatabase.Models.DTOModels;
using ApiToDatabase.Models.RequestModels;
using ApiToDatabase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiToDatabase.Controllers
{
    [Route("api/picture")]
    [ApiController]
    [Authorize]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureService _pictureService;
        public PicturesController(IPictureService pictureService)
        {
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
        public async Task<IActionResult> DeletePicture(string folderId, string pictureId) 
        {
            try
            {
                await _pictureService.DeletePicture(folderId, pictureId);
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
                var result = await _pictureService.CreatePicture(pictureRequest);
                return Created("/api/pictures", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
