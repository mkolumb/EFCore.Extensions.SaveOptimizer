using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark;
using EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark.Standard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark.Specific;

public static class BenchmarkHelper
{
    public static IWrapperResolver ContextResolver()
    {
        var connectionString = GetConnectionString();

        ServiceCollection collection = new();

        collection.AddDbContextFactory<EntitiesContext>(builder => builder.UseSqlServer(connectionString,
            cfg => cfg.CommandTimeout(600).MigrationsAssembly("EFCore.Extensions.SaveOptimizer.Model.SqlServer")));

        collection.AddSingleton<IWrapperResolver, WrapperResolver>();

        ServiceProvider provider = collection.BuildServiceProvider();

        IWrapperResolver resolver = provider.GetRequiredService<IWrapperResolver>();

        return resolver;
    }

    public static string GetConnectionString()
    {
        const string serverName = ".,1401";
        const string db = "test_db";
        const string user = "sa";
        const string password = "yourStrong(!)Password";

        return $"Server={serverName};Database={db};User Id={user};Password={password};";
    }
}
