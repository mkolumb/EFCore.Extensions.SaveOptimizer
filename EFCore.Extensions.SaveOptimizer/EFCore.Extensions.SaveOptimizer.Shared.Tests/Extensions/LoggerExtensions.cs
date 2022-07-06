using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

public static class LoggerExtensions
{
    public static void LogWithDate(this ILogger logger, string? text)
    {
        DateTime date = DateTime.Now;

        logger.Log(LogLevel.Warning, "[{date:HH:mm:ss}] {text}", date, text);
    }

    public static void WriteLineWithDate(this ITestOutputHelper logger, string? text)
    {
        DateTime date = DateTime.Now;

        logger.WriteLine($"[{date:HH:mm:ss}] {text}");
    }
}
