using System.Text.Json;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;

public class TestDataHelper
{
    private const string EnvironmentSettingsName = "environment.settings.json";
    private const string ProjectSearchExpression = "*.csproj";
    private const string ProjectName = "PROJECT_NAME";

    static TestDataHelper()
    {
        SetEnvironmentVariables();
        SetProjectName();
    }

    private static void SetEnvironmentVariables()
    {
        DirectoryInfo? directoryInfo = GetDirectory(EnvironmentSettingsName);

        if (directoryInfo == null)
        {
            return;
        }

        var path = Path.Combine(directoryInfo.FullName, EnvironmentSettingsName);

        FileInfo fileInfo = new(path);

        if (!fileInfo.Exists)
        {
            return;
        }

        using FileStream streamReader = fileInfo.OpenRead();

        IDictionary<string, string>? data = JsonSerializer.Deserialize<IDictionary<string, string>>(streamReader);

        if (data == null)
        {
            return;
        }

        foreach (var (key, value) in data)
        {
            SetVariable(key, value);
        }
    }

    private static void SetProjectName()
    {
        DirectoryInfo? directoryInfo = GetDirectory(ProjectSearchExpression);

        if (directoryInfo == null)
        {
            return;
        }

        FileInfo fileInfo = directoryInfo.EnumerateFiles(ProjectSearchExpression).First();

        SetVariable(ProjectName, fileInfo.Name);
    }

    private static DirectoryInfo? GetDirectory(string searchPattern)
    {
        DirectoryInfo? di = new(Directory.GetCurrentDirectory());

        while (di != null && !di.EnumerateFiles(searchPattern).Any())
        {
            di = di.Parent;
        }

        return di;
    }

    private static void SetVariable(string key, string value)
    {
        if (!Environment.GetEnvironmentVariables().Contains(key))
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }

    public static string? GetValue(string key) => Environment.GetEnvironmentVariables().Contains(key)
        ? Environment.GetEnvironmentVariable(key)
        : null;

    public static string[] GetValues(string key)
    {
        var value = GetValue(key);

        return value == null ? Array.Empty<string>() : value.Split(";");
    }

    public static bool IsDisabled(IEnumerable<string> disabledList) => disabledList
        .Any(p => !string.IsNullOrEmpty(p) &&
                  GetValue(ProjectName)!.Contains(p, StringComparison.InvariantCultureIgnoreCase));
}
