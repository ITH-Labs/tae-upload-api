namespace TAE.UploadService.Infrastructure
{
    public static class LoggerExtensions
    {
        public static void LogApiError(this Serilog.ILogger logger, Exception ex, string action, object? context = null)
        {
            if (context != null)
                logger.Error(ex, "Error in {Action} with context {@Context}", action, context);
            else
                logger.Error(ex, "Error in {Action}", action);
        }

        public static void LogApiInfo(this Serilog.ILogger logger, string message, string action, object? context = null)
        {
            if (context != null)
                logger.Information("Info in {Action}: {Message} {@Context}", action, message, context);
            else
                logger.Information("Info in {Action}: {Message}", action, message);
        }

        public static void LogApiDebug(this Serilog.ILogger logger, string message, string? action = null, object? context = null)
        {
            if (context != null)
                logger.Debug("Debug {Action}: {Message} {@Context}", action, message, context);
            else if (action != null)
                logger.Debug("Debug {Action}: {Message}", action, message);
            else
                logger.Debug("Debug: {Message}", message);
        }

        public static void LogApiWarning(this Serilog.ILogger logger, string message, string action, object? context = null)
        {
            if (context != null)
                logger.Warning("Warning in {Action}: {Message} {@Context}", action, message, context);
            else
                logger.Warning("Warning in {Action}: {Message}", action, message);
        }
    }
}
