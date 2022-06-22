using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.SqlLite;

public class SqlLiteDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args)
    {
        var connectionString = args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : GetConnectionString();

        DbContextOptions<EntitiesContext> options = new DbContextOptionsBuilder<EntitiesContext>()
            .UseSqlite(connectionString, cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.SqlLite"))
            .Options;

        return new EntitiesContext(options);
    }

    private string GetConnectionString()
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
