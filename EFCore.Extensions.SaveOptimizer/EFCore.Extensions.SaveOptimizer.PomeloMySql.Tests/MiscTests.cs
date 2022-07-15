using EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.PomeloMySql.Tests;

[Collection(Variables.ProviderName)]
public class MiscTests : BaseMiscTests
{
    public MiscTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
