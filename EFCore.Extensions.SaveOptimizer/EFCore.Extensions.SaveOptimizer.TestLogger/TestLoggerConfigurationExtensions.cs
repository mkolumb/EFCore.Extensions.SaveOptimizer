using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.TestLogger;

public static class TestLoggerConfigurationExtensions
{
    private static void AddTestLogger(this ILoggingBuilder builder, LogLevel minimumLogLevel) =>
        builder.Services.AddSingleton<ILoggerProvider>(c =>
            new TestLoggerProvider(c.GetRequiredService<ITestOutputHelper>(), minimumLogLevel));

    public static IServiceCollection AddTestLogger(this IServiceCollection services, ITestOutputHelper testOutputHelper,
        LogLevel minimumLogLevel)
    {
        services.AddSingleton(testOutputHelper);

        services.AddLogging(c => c.ClearProviders().AddTestLogger(minimumLogLevel));

        return services;
    }
}
