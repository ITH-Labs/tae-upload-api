using Serilog.Core;
using Serilog.Events;

namespace TAE.UploadService.Tests.Infrastructure
{
    public class TestLogger : ILogEventSink
    {
        public List<LogEvent> Events { get; } = [];
        public void Emit(LogEvent logEvent)
        {
            Events.Add(logEvent);
        }
    }
}