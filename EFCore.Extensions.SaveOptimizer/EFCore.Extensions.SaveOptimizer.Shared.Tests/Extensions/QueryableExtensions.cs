using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

public static class QueryableExtensions
{
    private const int RunTry = 10;

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
