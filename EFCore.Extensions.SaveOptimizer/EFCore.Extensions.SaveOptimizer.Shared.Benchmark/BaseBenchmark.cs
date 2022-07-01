using System.Management.Automation;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Loggers;
using EFCore.Extensions.SaveOptimizer.Shared.Benchmark.Exporter;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseBenchmark
{
    private const int MaxPrepareTry = 5;
    private readonly IWrapperResolver _contextResolver;
    protected IDbContextWrapper? Context;
    protected int Iterations;

    public abstract string Database { get; }

    public abstract string Operation { get; }

    public abstract long Rows { get; set; }

    [Params(SaveVariant.Optimized, SaveVariant.OptimizedDapper, SaveVariant.EfCore)]
    public SaveVariant Variant { get; set; }

    protected BaseBenchmark(IWrapperResolver contextResolver) => _contextResolver = contextResolver;

    [GlobalSetup]
    public async Task Setup()
    {
        ConsoleLogger.Unicode.WriteLineHint($"Setup {GetDescription()}");

        RestartContainer();

        Context = _contextResolver.Resolve();

        await Context.Seed(Rows * BenchmarkConfig.GetSeedRepeat(), 1);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        Iterations++;

        ConsoleLogger.Unicode.WriteLineHint($"Iteration setup {Iterations} {GetDescription()}");

        var i = 0;

        while (i < MaxPrepareTry)
        {
            try
            {
                Prepare();

                return;
            }
            catch
            {
                i++;
            }
        }

        throw new Exception($"Unable to prepare iteration {Iterations} {GetDescription()}");
    }

    protected abstract void Prepare();

    [GlobalCleanup]
    public async Task Cleanup()
    {
        ConsoleLogger.Unicode.WriteLineHint($"Cleanup {GetDescription()}");

        if (Context != null)
        {
            await Context.Truncate();

            Context.Dispose();
        }
    }

    private void RestartContainer()
    {
        DirectoryInfo? dir = GetScriptsDirectory();

        var shouldRecreate = false;

        var description = $"{Database} {Operation} {Variant} / {Rows / 100}".Trim();

        if (dir != null)
        {
            FileInfo fi = new(Path.Combine(dir.FullName, "bin", "check.txt"));

            if (fi.Exists)
            {
                var content = File.ReadAllText(fi.FullName).Trim();

                if (content != description)
                {
                    shouldRecreate = true;
                }
            }

            File.WriteAllLines(fi.FullName, new[] { description }, Encoding.UTF8);
        }

        if (!shouldRecreate)
        {
            return;
        }

        StopContainer();

        StartContainer();
    }

    private static void StartContainer()
    {
        DirectoryInfo? di = GetScriptsDirectory();

        if (di == null)
        {
            throw new ArgumentException("Unable to find proper PowerShell script");
        }

        RunScript(di, "start.ps1");
    }

    private static void StopContainer()
    {
        DirectoryInfo? di = GetScriptsDirectory();

        if (di == null)
        {
            throw new ArgumentException("Unable to find proper PowerShell script");
        }

        RunScript(di, "stop.ps1");
    }

    private static void RunScript(FileSystemInfo di, string scriptPath)
    {
        var path = Path.Combine(di.FullName, scriptPath);

        var cmd = $@"& '{path}'";

        ConsoleLogger.Unicode.WriteLineHint($"Running {path}");

        ConsoleLogger.Unicode.WriteLineHint(cmd);

        using (PowerShell? powerShell = PowerShell.Create(RunspaceMode.NewRunspace))
        {
            powerShell.Streams.Information.DataAdded += LogProgress<InformationRecord>;
            powerShell.Streams.Warning.DataAdded += LogProgress<WarningRecord>;
            powerShell.Streams.Error.DataAdded += LogProgress<ErrorRecord>;
            powerShell.Streams.Verbose.DataAdded += LogProgress<VerboseRecord>;
            powerShell.Streams.Debug.DataAdded += LogProgress<DebugRecord>;

            powerShell.AddScript(cmd);

            powerShell.Invoke();
        }

        ConsoleLogger.Unicode.WriteLineHint($"Finished {path}");
    }

    private static void LogProgress<T>(object? sender, DataAddedEventArgs e)
    {
        T? data = (sender as PSDataCollection<T>)![e.Index];

        ConsoleLogger.Unicode.WriteLineHint($"[{typeof(T).Name}] {Convert.ToString(data)}");
    }

    private static DirectoryInfo? GetScriptsDirectory()
    {
        DirectoryInfo? di = new(Directory.GetCurrentDirectory());

        while (di != null && !di.EnumerateFiles("start.ps1").Any())
        {
            di = di.Parent;
        }

        return di;
    }

    protected string GetDescription() => $"{Database} {Operation} {Variant} {Rows} {DateTimeOffset.UtcNow:T}";
}
