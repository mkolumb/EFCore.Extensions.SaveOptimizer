using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

public static class LoggerExtensions
{
    public static void LogWithDate(this ILogger logger, string? text)
    {
        DateTime date = DateTime.Now;

        logger.Log(LogLevel.Information, "[{date:HH:mm:ss}] {text}", date, text);
    }
}
