using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

public static class LoggerExtensions
{
    private const int RunTry = 10;

    public static void LogWithDate(this ILogger logger, string? text)
    {
        DateTime date = DateTime.Now;

        logger.Log(LogLevel.Warning, "[{date:HH:mm:ss}] {text}", date, text);
    }

    public static async Task<T[]> ToArrayWithRetry<T>(this IQueryable<T> source) =>
        await Run(RunTry, () => source.ToArrayAsync());

    private static async Task<T> Run<T>(int max, Func<Task<T>> method)
    {
        var i = 0;

        while (i < max)
        {
            try
            {
                return await method();
            }
            catch
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                i++;
            }
        }

        throw new Exception("Unable to run method");
    }
}
