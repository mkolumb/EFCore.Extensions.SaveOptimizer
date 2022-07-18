using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Benchmark.Standard;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Benchmark.Specific;

public static class BenchmarkHelper
{
    public static IWrapperResolver ContextResolver()
    {
        var connectionString = GetConnectionString();

        ServiceCollection collection = new();

        collection.AddDbContextFactory<EntitiesContext>(builder => builder
            .UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString),
                cfg => cfg.CommandTimeout(600)
                    .MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb")));

        collection.AddSingleton<IWrapperResolver, WrapperResolver>();

        ServiceProvider provider = collection.BuildServiceProvider();

        IWrapperResolver resolver = provider.GetRequiredService<IWrapperResolver>();

        return resolver;
    }

    public static string GetConnectionString()
    {
        const string serverName = "127.0.0.1";
        const int port = 3307;
        const string db = "test_db";
        const string user = "root";
        const string password = "root";

        return $"Server={serverName};Port={port};Database={db};User Id={user};Password={password};";
    }
}
