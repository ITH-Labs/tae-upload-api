using Microsoft.AspNetCore.Mvc;
using TAE.UploadService.Infrastructure;
using TAE.UploadService.Repositories;

namespace TAE.UploadService.Controllers
{
    [ApiController]
    [Route("api/v1/fileupload")]
    public class UploadController(UploadRepository uploadRepository, Serilog.ILogger logger) : ControllerBase
    {
        private readonly UploadRepository _uploadRepository = uploadRepository;
        protected readonly Serilog.ILogger log = logger;

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            try
            {
                log.LogApiInfo("Upload called", nameof(Upload));
                var result = await _uploadRepository.UploadFileAsync(file);

                return Ok(result);
            }
            catch (Exception ex)
            {
                log.LogApiError(ex, nameof(Upload));
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("upload/{fileId}/status")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public virtual async Task<ActionResult<string>> Status(int fileId)
        {
            try
            {
                log.LogApiInfo($"Status called with file status: {fileId}", nameof(Status));
                // logic
                return Ok();
            }
            catch (Exception ex)
            {
                log.LogApiError(ex, nameof(Status));
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("health")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public virtual IActionResult Health()
        {
            try
            {
                log.LogApiInfo("Health check called", nameof(Health));
                return Ok(new { status = "ok" });
            }
            catch (Exception ex)
            {
                log.LogApiError(ex, nameof(Health));
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
