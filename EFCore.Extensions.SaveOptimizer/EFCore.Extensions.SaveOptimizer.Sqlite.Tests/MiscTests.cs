using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Sqlite.Tests;

[Collection("Sqlite")]
public class MiscTests : BaseMiscTests
{
    public MiscTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
