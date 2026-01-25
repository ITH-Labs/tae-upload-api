using Serilog;
using Serilog.Events;
using TAE.UploadService.Infrastructure;

namespace TAE.UploadService.Tests.Infrastructure
{
    public class LoggerExtensionsTests
    {
        private (ILogger logger, TestLogger sink) CreateTestLogger()
        {
            var sink = new TestLogger();
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(sink)
                .CreateLogger();

            return (logger, sink);
        }

        [Fact]
        public void LogApiError_WithoutContext_LogsError()
        {
            var (logger, sink) = CreateTestLogger();
            var ex = new Exception("boom");

            logger.LogApiError(ex, "Health");

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Equal(LogEventLevel.Error, evt.Level);
            Assert.Contains("Error in \"Health\"", evt.RenderMessage());
            Assert.Equal(ex, evt.Exception);
        }

        [Fact]
        public void LogApiError_WithContext_LogsErrorAndContext()
        {
            var (logger, sink) = CreateTestLogger();
            var ctx = new { FileId = 10 };
            var ex = new Exception("boom");

            logger.LogApiError(ex, "Upload", ctx);

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Equal(LogEventLevel.Error, evt.Level);
            Assert.Contains("Error in \"Upload\"", evt.RenderMessage());
            Assert.Contains("FileId", evt.RenderMessage());
            Assert.Equal(ex, evt.Exception);
        }

        [Fact]
        public void LogApiInfo_WithoutContext_LogsInfo()
        {
            var (logger, sink) = CreateTestLogger();

            logger.LogApiInfo("Started", "Upload");

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Equal(LogEventLevel.Information, evt.Level);
            Assert.Contains("Info in \"Upload\": \"Started\"", evt.RenderMessage());
        }

        [Fact]
        public void LogApiInfo_WithContext_LogsInfoAndContext()
        {
            var (logger, sink) = CreateTestLogger();
            var ctx = new { Id = 1 };

            logger.LogApiInfo("Processing", "Status", ctx);

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Equal(LogEventLevel.Information, evt.Level);
            Assert.Contains("Processing", evt.RenderMessage());
            Assert.Contains("Id", evt.RenderMessage());
        }

        [Fact]
        public void LogApiDebug_NoAction_LogsSimplestDebug()
        {
            var (logger, sink) = CreateTestLogger();

            logger.LogApiDebug("Something happened");

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Equal(LogEventLevel.Debug, evt.Level);
            Assert.Contains("Something happened", evt.RenderMessage());
        }

        [Fact]
        public void LogApiDebug_WithAction_NoContext_LogsCorrectFormat()
        {
            var (logger, sink) = CreateTestLogger();

            logger.LogApiDebug("Test message", "Upload");

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            // Updated for Serilog structured rendering
            Assert.Contains("Debug \"Upload\": \"Test message\"", evt.RenderMessage());
        }

        [Fact]
        public void LogApiDebug_WithActionAndContext_LogsContext()
        {
            var (logger, sink) = CreateTestLogger();
            var ctx = new { Name = "Test" };

            logger.LogApiDebug("Hello", "Upload", ctx);

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Contains("Hello", evt.RenderMessage());
            Assert.Contains("Name", evt.RenderMessage());
        }

        [Fact]
        public void LogApiWarning_WithoutContext_LogsWarning()
        {
            var (logger, sink) = CreateTestLogger();

            logger.LogApiWarning("Be careful", "Upload");

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Equal(LogEventLevel.Warning, evt.Level);
            Assert.Contains("Be careful", evt.RenderMessage());
        }

        [Fact]
        public void LogApiWarning_WithContext_LogsWarningAndContext()
        {
            var (logger, sink) = CreateTestLogger();
            var ctx = new { Value = 123 };

            logger.LogApiWarning("Look out!", "Upload", ctx);

            Assert.Single(sink.Events);
            var evt = sink.Events[0];

            Assert.Equal(LogEventLevel.Warning, evt.Level);
            Assert.Contains("Look out!", evt.RenderMessage());
            Assert.Contains("Value", evt.RenderMessage());
        }
    }
}