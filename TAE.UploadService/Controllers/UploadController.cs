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
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            const long maxSizeBytes = 10 * 1024 * 1024; // 10MB
            if (file.Length > maxSizeBytes)
            {
                log.LogApiWarning("File size exceeds limit", nameof(Upload), new { fileSize = file.Length });
                return Problem(
                    detail: "File exceeds maximum allowed size.",
                    statusCode: StatusCodes.Status413PayloadTooLarge
                );
            }

            var allowedExtensions = new[] { ".xlsx" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                log.LogApiWarning("Unsupported file type", nameof(Upload), new { fileExtension = extension });
                return Problem(
                    detail: "Only .xlsx files are supported.",
                    statusCode: StatusCodes.Status415UnsupportedMediaType
                );
            }

            try
            {
                log.LogApiInfo("Upload called", nameof(Upload));
                var uploadId = await _uploadRepository.StoreAsync(file);
                return Ok(uploadId);
            }
            catch (InvalidDataException ex)
            {
                log.LogApiError(ex, nameof(Upload));
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                log.LogApiError(ex, nameof(Upload));
                return Problem(
                    detail: "Unexpected error while processing upload.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
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
