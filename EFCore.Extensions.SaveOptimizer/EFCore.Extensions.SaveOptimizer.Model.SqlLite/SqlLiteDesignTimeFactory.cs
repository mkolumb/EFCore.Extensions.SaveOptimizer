using EFCore.Extensions.SaveOptimizer.Interceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.SqlLite;

public class SqlLiteDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args)
    {
        DirectoryInfo directory = new("db");

        if (!directory.Exists)
        {
            directory.Create();
        }

        var path = Path.Join(directory.FullName, $"test_{DateTime.Now:yyyy_MM_dd_hh_mm_ss}.db");

        SaveChangesOptimizerInterceptor interceptor = new();

        DbContextOptions<EntitiesContext> options = new DbContextOptionsBuilder<EntitiesContext>()
            .UseSqlite($"Data Source={path}",
                cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.SqlLite"))
            .AddInterceptors(interceptor)
            .Options;

        return new EntitiesContext(options);
    }
}
