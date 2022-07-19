using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Model.Factories;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

#pragma warning disable EF1001

namespace EFCore.Extensions.SaveOptimizer.Model.Firebird3;

public class Firebird3DesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>,
    ITestTimeDbContextFactory<EntitiesContext>
{
    static Firebird3DesignTimeFactory()
    {
        static void Builder(ModelBuilder modelBuilder)
        {
            const string decimalColumnType = "DECIMAL(12,6)";

            modelBuilder.Entity<NonRelatedEntity>(eb =>
            {
                eb.Property(b => b.NullableDecimal)
                    .HasPrecision(12, 6)
                    .HasColumnType(decimalColumnType);

                eb.Property(b => b.NonNullableDecimal)
                    .HasPrecision(12, 6)
                    .HasColumnType(decimalColumnType);
            });

            modelBuilder.Entity<VariousTypeEntity>(eb =>
            {
                eb.Property(b => b.SomeDecimal)
                    .HasPrecision(12, 6)
                    .HasColumnType(decimalColumnType);
            });

            modelBuilder.Entity<AutoIncrementEntity>(eb =>
            {
                eb.Property(b => b.Id)
                    .HasAnnotation(FbAnnotationNames.ValueGenerationStrategy, FbValueGenerationStrategy.IdentityColumn);
            });

            modelBuilder.Entity<FailingEntity>(eb =>
            {
                eb.Property(b => b.Id)
                    .HasAnnotation(FbAnnotationNames.ValueGenerationStrategy, FbValueGenerationStrategy.IdentityColumn);
            });
        }

        EntitiesContext.AdditionalBuilders.Add(Builder);
    }

    public EntitiesContext CreateDbContext(string[] args) => CreateDbContext(args, null);

    public EntitiesContext CreateDbContext(string[] args, ILoggerFactory? factory)
    {
        var connectionString = args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : GetConnectionString();

        DbContextOptionsBuilder<EntitiesContext> builder = new DbContextOptionsBuilder<EntitiesContext>()
            .UseFirebird(connectionString,
                cfg => cfg.CommandTimeout(600).MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.Firebird3"));

        if (factory != null)
        {
            builder = builder.UseLoggerFactory(factory);
        }

        return new EntitiesContext(builder.Options);
    }

    private static string GetConnectionString()
    {
        const string serverName = "127.0.0.1";
        const int port = 3050;
        const string db = "test_db";
        const string user = "sysdba";
        const string password = "root";

        return $"Server={serverName};Port={port};Database={db};User Id={user};Password={password};";
    }
}
