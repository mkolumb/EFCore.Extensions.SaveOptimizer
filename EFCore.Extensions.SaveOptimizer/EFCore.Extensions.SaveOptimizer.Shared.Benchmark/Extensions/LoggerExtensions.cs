using BenchmarkDotNet.Loggers;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Extensions;

public static class LoggerExtensions
{
    public static void WriteLineWithDate(this ILogger logger, string? text)
    {
        logger.WriteLineHint($"[{DateTime.Now:HH:mm:ss}] {text}");
    }
}
