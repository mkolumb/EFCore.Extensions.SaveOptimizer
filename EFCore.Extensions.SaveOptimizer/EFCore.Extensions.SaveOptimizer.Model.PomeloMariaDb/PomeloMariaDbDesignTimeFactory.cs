﻿using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Model.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb;

public class PomeloMariaDbDesignTimeFactory : IDesignTimeDbContextFactory<EntitiesContext>,
    ITestTimeDbContextFactory<EntitiesContext>
{
    public EntitiesContext CreateDbContext(string[] args) => CreateDbContext(args, null);

    public EntitiesContext CreateDbContext(string[] args, ILoggerFactory? factory)
    {
        var connectionString = args.Any() && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : GetConnectionString();

        DbContextOptionsBuilder<EntitiesContext> builder = new DbContextOptionsBuilder<EntitiesContext>()
            .UseMySql(connectionString,
                new MariaDbServerVersion(new Version(10, 8)),
                cfg => cfg.CommandTimeout(600)
                    .MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb"));

        if (factory != null)
        {
            builder = builder.UseLoggerFactory(factory);
        }

        return new EntitiesContext(builder.Options);
    }

    private static string GetConnectionString()
    {
        const string serverName = "127.0.0.1";
        const int port = 3307;
        const string db = "test_db";
        const string user = "root";
        const string password = "root";

        return $"Server={serverName};Port={port};Database={db};User Id={user};Password={password};";
    }
}
