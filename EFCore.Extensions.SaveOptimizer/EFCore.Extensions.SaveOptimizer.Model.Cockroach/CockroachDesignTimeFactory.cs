using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Npgsql;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.Cockroach;

public class CockroachDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>,
    ITestTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args) => CreateDbContext(args, null);

    public EntitiesContext CreateDbContext(string[] args, ILoggerFactory? factory)
    {
        DbConnection connection = GetConnection();

        DbContextOptionsBuilder<EntitiesContext> builder = new DbContextOptionsBuilder<EntitiesContext>()
            .UseNpgsql(connection, cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.Cockroach"));

        if (factory != null)
        {
            builder = builder.UseLoggerFactory(factory);
        }

        return new EntitiesContext(builder.Options);
    }

    private static DbConnection GetConnection()
    {
        const string connectionString = "Host=localhost;Port=26258;SSL Mode=Disable;Username=root;Database=test_db";

        NpgsqlConnection conn = new(connectionString);

        conn.UserCertificateValidationCallback += (_, _, _, _) => true;

        return conn;
    }
}
