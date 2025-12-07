using Microsoft.AspNetCore.Mvc;
using TAE.UploadService.Repositories;

namespace TAE.UploadService.Controllers
{
    [ApiController]
    [Route("api/v1/fileupload")]
    public class UploadController(UploadRepository uploadRepository) : ControllerBase
    {
        private readonly UploadRepository _uploadRepository = uploadRepository;

        [HttpPost]
        [Route("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            try
            {
                var result = await _uploadRepository.UploadFileAsync(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("upload/{fileId}/status")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Status(int fileId)
        {
            try
            {
                // logic
                return Ok();                
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("health")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public IActionResult Health()
        {
            try
            {
                return Ok(new { status = "ok" });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
