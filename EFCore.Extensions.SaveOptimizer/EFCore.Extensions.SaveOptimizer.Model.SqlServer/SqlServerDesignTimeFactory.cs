using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.SqlServer;

public class SqlServerDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args)
    {
        var connectionString = args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : GetConnectionString();

        DbContextOptions<EntitiesContext> options = new DbContextOptionsBuilder<EntitiesContext>()
            .UseSqlServer(connectionString,
                cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.SqlServer"))
            .Options;

        return new EntitiesContext(options);
    }

    private static string GetConnectionString()
    {
        const string serverName = ".,1401";
        const string db = "test_db";
        const string user = "sa";
        const string password = "yourStrong(!)Password";

        return $"Server={serverName};Database={db};User Id={user};Password={password};";
    }
}
