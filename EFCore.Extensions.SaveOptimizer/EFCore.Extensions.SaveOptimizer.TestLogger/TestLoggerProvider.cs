using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.TestLogger;

public class TestLoggerProvider : ILoggerProvider
{
    private readonly LogLevel _minimumLogLevel;
    private readonly ITestOutputHelper? _testOutputHelper;

    public TestLoggerProvider(ITestOutputHelper? testOutputHelper, LogLevel minimumLogLevel)
    {
        _testOutputHelper = testOutputHelper;
        _minimumLogLevel = minimumLogLevel;
    }

    public void Dispose() => GC.SuppressFinalize(this);

    public ILogger CreateLogger(string categoryName) =>
        new TestLogger(_testOutputHelper, categoryName, _minimumLogLevel);
}
