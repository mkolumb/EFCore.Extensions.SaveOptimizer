using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.TestLogger
{
    public class TestLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly LogLevel _minimumLogLevel;

        public TestLoggerProvider(ITestOutputHelper testOutputHelper, LogLevel minimumLogLevel)
        {
            _testOutputHelper = testOutputHelper;
            _minimumLogLevel = minimumLogLevel;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestLogger(_testOutputHelper, categoryName, _minimumLogLevel);
        }
    }
}
