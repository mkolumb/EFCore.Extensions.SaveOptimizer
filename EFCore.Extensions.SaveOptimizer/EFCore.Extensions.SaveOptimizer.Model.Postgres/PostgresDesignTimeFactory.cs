using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.Postgres;

public class PostgresDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args)
    {
        DbConnection connection = GetConnection();

        DbContextOptions<EntitiesContext> options = new DbContextOptionsBuilder<EntitiesContext>()
            .UseNpgsql(connection, cfg => cfg.MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.Postgres"))
            .Options;

        return new EntitiesContext(options);
    }

    private static DbConnection GetConnection()
    {
        const string connectionString = "Host=localhost;Port=26257;SSL Mode=Disable;Username=root;Database=test_db";

        NpgsqlConnection conn = new(connectionString);

        conn.UserCertificateValidationCallback += (_, _, _, _) => true;

        return conn;
    }
}
