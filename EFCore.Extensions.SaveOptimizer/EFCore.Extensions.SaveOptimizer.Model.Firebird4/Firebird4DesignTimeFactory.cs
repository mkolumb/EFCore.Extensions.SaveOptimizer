using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.Firebird4;

public class Firebird4DesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>,
    ITestTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args) => CreateDbContext(args, null);

    public EntitiesContext CreateDbContext(string[] args, ILoggerFactory? factory)
    {
        var connectionString = args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : GetConnectionString();

        DbContextOptionsBuilder<EntitiesContext> builder = new DbContextOptionsBuilder<EntitiesContext>()
            .UseFirebird(connectionString,
                cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.Firebird4"));

        if (factory != null)
        {
            builder = builder.UseLoggerFactory(factory);
        }

        return new EntitiesContext(builder.Options);
    }

    private static string GetConnectionString()
    {
        const string serverName = "127.0.0.1";
        const int port = 3051;
        const string db = "test_db";
        const string user = "sysdba";
        const string password = "root";

        return $"Server={serverName};Port={port};Database={db};User Id={user};Password={password};";
    }
}
