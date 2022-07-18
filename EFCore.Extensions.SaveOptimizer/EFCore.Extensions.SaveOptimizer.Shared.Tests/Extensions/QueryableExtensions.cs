using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

public static class QueryableExtensions
{
    private const int RunTry = 10;

    public static async Task<T[]> ToArrayWithRetryAsync<T>(this IQueryable<T> source) =>
        await RunAsync(RunTry, () => source.ToArrayAsync()).ConfigureAwait(false);

    public static T[] ToArrayWithRetry<T>(this IQueryable<T> source) => Run(RunTry, source.ToArray);

    private static async Task<T> RunAsync<T>(int max, Func<Task<T>> method)
    {
        var i = 0;

        do
        {
            try
            {
                return await method().ConfigureAwait(false);
            }
            catch
            {
                i++;

                if (i >= max)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        } while (i < max);

        throw new Exception("Unable to run method - something weird happened");
    }

    private static T Run<T>(int max, Func<T> method)
    {
        var i = 0;

        do
        {
            try
            {
                return method();
            }
            catch
            {
                i++;

                if (i >= max)
                {
                    throw;
                }

                Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        } while (i < max);

        throw new Exception("Unable to run method - something weird happened");
    }
}
