using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.TestLogger
{
    public class TestLogger : ILogger
    {
        private readonly string _category;
        private readonly LogLevel _minimumLogLevel;
        private readonly ITestOutputHelper _testOutputHelper;

        public TestLogger(ITestOutputHelper testOutputHelper, string category,
            LogLevel minimumLogLevel = LogLevel.Trace)
        {
            _testOutputHelper = testOutputHelper;
            _category = category;
            _minimumLogLevel = minimumLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var data = formatter(state, exception);

            _testOutputHelper.WriteLine(exception != null
                ? $"{DateTime.Now:HH:mm:ss.ffff} | {logLevel} | {_category} | {data} | exception: {exception}"
                : $"{DateTime.Now:HH:mm:ss.ffff} | {logLevel} | {_category} | {data}");
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return default!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minimumLogLevel;
        }
    }
}
