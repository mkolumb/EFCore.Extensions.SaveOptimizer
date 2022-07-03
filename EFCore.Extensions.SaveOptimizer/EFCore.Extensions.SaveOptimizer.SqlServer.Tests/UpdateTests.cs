using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.SqlServer.Tests;

[Collection("SqlServer")]
public class UpdateTests : BaseUpdateTests
{
    public UpdateTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
