﻿using EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark.Standard;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark.Specific;

public static class BenchmarkHelper
{
    public static IWrapperResolver ContextResolver()
    {
        var connectionString = GetConnectionString();

        ServiceCollection collection = new();

        collection.AddDbContextFactory<EntitiesContext>(builder => builder
            .UseFirebird(connectionString,
                cfg => cfg.CommandTimeout(600).MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.Firebird3")));

        collection.AddSingleton<IWrapperResolver, WrapperResolver>();

        ServiceProvider provider = collection.BuildServiceProvider();

        IWrapperResolver resolver = provider.GetRequiredService<IWrapperResolver>();

        return resolver;
    }

    public static string GetConnectionString()
    {
        const string serverName = "127.0.0.1";
        const int port = 3050;
        const string db = "test_db";
        const string user = "sysdba";
        const string password = "root";

        return $"Server={serverName};Port={port};Database={db};User Id={user};Password={password};";
    }
}
