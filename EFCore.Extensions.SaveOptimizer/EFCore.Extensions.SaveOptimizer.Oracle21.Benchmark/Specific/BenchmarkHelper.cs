﻿using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Oracle21.Benchmark.Standard;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace EFCore.Extensions.SaveOptimizer.Oracle21.Benchmark.Specific;

public static class BenchmarkHelper
{
    public static IWrapperResolver ContextResolver()
    {
        var connectionString = GetConnectionString();

        ServiceCollection collection = new();

        collection.AddDbContextFactory<EntitiesContext>(builder => builder
            .UseOracle(connectionString,
                cfg => cfg.CommandTimeout(600).MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.Oracle21")));

        collection.AddSingleton<IWrapperResolver, WrapperResolver>();

        ServiceProvider provider = collection.BuildServiceProvider();

        IWrapperResolver resolver = provider.GetRequiredService<IWrapperResolver>();

        return resolver;
    }

    public static string GetConnectionString()
    {
        const string serverName = "127.0.0.1";
        const int port = 1521;
        const string service = "XE";
        const string user = "system";
        const string password = "root";

        var dataSource = $"{serverName}:{port}/{service}";

        OracleConnectionStringBuilder builder = new() { DataSource = dataSource, UserID = user, Password = password };

        return builder.ToString();
    }
}
