using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder<T> UseDynamicSqlLite<T>(this DbContextOptionsBuilder<T> builder) where T : DbContext
    {
        return builder
            .UseSqlite(GetConnectionString())
            .UseSnakeCaseNamingConvention();
    }

    private static string GetConnectionString()
    {
        DirectoryInfo directory = new("db");

        if (!directory.Exists)
        {
            directory.Create();
        }

        var dbName = $"test_{DateTime.Now:yyyy_MM_dd_hh_mm_ss}_{Guid.NewGuid()}.db";

        var path = Path.Join(directory.FullName, dbName);

        return $"Data Source={path}";
    }
}
