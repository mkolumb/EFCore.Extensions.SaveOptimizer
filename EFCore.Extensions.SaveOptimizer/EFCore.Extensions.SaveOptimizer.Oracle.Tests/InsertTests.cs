using EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Oracle.Tests;

[Collection(Variables.ProviderName)]
public class InsertTests : BaseInsertTests
{
    public InsertTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
