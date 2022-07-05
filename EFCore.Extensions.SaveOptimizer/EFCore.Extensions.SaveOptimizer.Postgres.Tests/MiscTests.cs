using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Postgres.Tests;

[Collection("Postgres")]
public class MiscTests : BaseMiscTests
{
    public MiscTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
