using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.PomeloMySql;

public class PomeloMySqlDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args)
    {
        var connectionString = args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : GetConnectionString();

        DbContextOptions<EntitiesContext> options = new DbContextOptionsBuilder<EntitiesContext>()
            .UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString),
                cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.PomeloMySql"))
            .Options;

        return new EntitiesContext(options);
    }

    private static string GetConnectionString()
    {
        const string serverName = "127.0.0.1";
        const int port = 3306;
        const string db = "test_db";
        const string user = "root";
        const string password = "root";

        return $"Server={serverName};Port={port};Database={db};User Id={user};Password={password};";
    }
}
