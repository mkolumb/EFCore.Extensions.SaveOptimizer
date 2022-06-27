using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.TestLogger;

public class TestLogger<T> : TestLogger, ILogger<T>
{
    public TestLogger(ITestOutputHelper testOutputHelper, LogLevel minimumLogLevel = LogLevel.Trace)
        : base(testOutputHelper, typeof(T).Name, minimumLogLevel)
    {
    }
}
