using Serilog;

namespace TAE.UploadService.Infrastructure
{
    public static class Logger
    {
        public static void Configure(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", "TAE-Upload-Api")
                .CreateLogger();
        }
    }
}
