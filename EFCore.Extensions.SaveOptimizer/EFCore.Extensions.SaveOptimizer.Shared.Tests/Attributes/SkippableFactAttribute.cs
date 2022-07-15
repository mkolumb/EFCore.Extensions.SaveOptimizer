using EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;

public sealed class SkippableFactAttribute : FactAttribute
{
    private const string TestDisabledProviders = "TEST_DISABLED_PROVIDERS";

    public SkippableFactAttribute()
    {
        if (ShouldSkip())
        {
            Skip = $"This is skipped due to {TestDisabledProviders} environment variable";
        }
    }

    private static bool ShouldSkip()
    {
        var providers = TestDataHelper.GetValues(TestDisabledProviders);

        return TestDataHelper.IsDisabled(providers);
    }
}
