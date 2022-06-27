using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.SqlLite;

public class SqlLiteDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>,
    ITestTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args) => CreateDbContext(args, null);

    public EntitiesContext CreateDbContext(string[] args, ILoggerFactory? factory)
    {
        var connectionString = args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : GetConnectionString();

        DbContextOptionsBuilder<EntitiesContext> builder = new DbContextOptionsBuilder<EntitiesContext>()
            .UseSqlite(connectionString,
                cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.SqlLite"));

        if (factory != null)
        {
            builder = builder.UseLoggerFactory(factory);
        }

        return new EntitiesContext(builder.Options);
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
