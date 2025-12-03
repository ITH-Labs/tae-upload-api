using Microsoft.AspNetCore.Mvc;

namespace TAE.UploadService.Controllers
{
    [ApiController]
    [Route("api/v1/fileupload")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Upload()
        {
            try
            {
                // logic
                return "";
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        [HttpGet]
        [Route("upload/{fileId}/status")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Status(int fileId)
        {
            try
            {
                // logic
                return "";
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        [HttpGet]
        [Route("health")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Health()
        {
            try
                {// test format
                // logic
                return "";
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }
    }
}
