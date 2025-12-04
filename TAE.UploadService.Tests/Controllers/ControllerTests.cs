using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TAE.UploadService.Controllers;
using Xunit;

namespace TAE.UploadService.Tests.Controllers
{
    public class ControllerTests(WebApplicationFactory<UploadController> webApplicationFactory) : IClassFixture<WebApplicationFactory<UploadController>>
    {
        private readonly WebApplicationFactory<UploadController> _webApplicationFactory = webApplicationFactory;

        #region Upload Endpoint Tests
        [Fact]
        public void Upload_Method_Exists()
        {
            var controllerType = typeof(UploadController);
            var methodInfo = controllerType.GetMethod("Upload");

            Assert.NotNull(methodInfo);
            Assert.Equal(typeof(Task<ActionResult<string>>), methodInfo.ReturnType);
        }

        [Fact]
        public async Task Upload_Returns_Ok()
        {
            var client = _webApplicationFactory.CreateClient();

            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent([1, 2, 3]);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, "file", "test.txt");

            var response = await client.PostAsync("/api/v1/fileupload/upload", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion

        #region Status Endpoint Tests
        [Fact]
        public void Status_Method_Exists()
        {
            var controllerType = typeof(UploadController);
            var methodInfo = controllerType.GetMethod("Status");

            Assert.NotNull(methodInfo);
            Assert.Equal(typeof(Task<ActionResult<string>>), methodInfo.ReturnType);
        }

        [Fact]
        public async Task Status_Returns_Ok()
        {
            var client = _webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/api/v1/fileupload/upload/1/status");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion

        #region Health Endpoint Tests
        [Fact]
        public void Health_Method_Exists()
        {
            var method = typeof(UploadController).GetMethod("Health");

            Assert.NotNull(method);
            Assert.Equal(typeof(IActionResult), method.ReturnType);
        }

        [Fact]
        public async Task Health_Returns_Ok()
        {
            var client = _webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/api/v1/fileupload/health");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Healh_Returns_StatusOk_InBody()
        {
            var client = _webApplicationFactory.CreateClient();
            var response = await client.GetAsync("/api/v1/fileupload/health");

            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            Assert.NotNull(content);
            Assert.True(content.ContainsKey("status"));
            Assert.Equal("ok", content["status"]);
        }

        [Fact]
        public async Task Health_Response_Is_Quick()
        {
            var client = _webApplicationFactory.CreateClient();
            var stopwatch = Stopwatch.StartNew();
            _ = await client.GetAsync("/api/v1/fileupload/health");

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds < 100, "Health endpoint is too slow");
        }
        #endregion
    }
}