using Microsoft.AspNetCore.Mvc;
using TAE.UploadService.Controllers;

namespace TAE.UploadService.Tests.Controllers
{
    public class ControllerTests
    {
        [Fact]
        public void UploadController_Upload_Method_Exists()
        {
            var controllerType = typeof(UploadController);
            var methodInfo = controllerType.GetMethod("Upload");

            Assert.NotNull(methodInfo);
            Assert.Equal(typeof(Task<ActionResult<string>>), methodInfo.ReturnType);
        }

        [Fact]
        public void UploadController_Status_Method_Exists()
        {
            var controllerType = typeof(UploadController);
            var methodInfo = controllerType.GetMethod("Status");

            Assert.NotNull(methodInfo);
            Assert.Equal(typeof(Task<ActionResult<string>>), methodInfo.ReturnType);
        }

        [Fact]
        public void UploadController_Health_Method_Exists()
        {
            var controllerType = typeof(UploadController);
            var methodInfo = controllerType.GetMethod("Health");

            Assert.NotNull(methodInfo);
            Assert.Equal(typeof(Task<ActionResult<string>>), methodInfo.ReturnType);
        }

    }
}