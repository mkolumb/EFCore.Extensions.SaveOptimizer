using EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.CockroachMulti.Tests;

[Collection(Variables.ProviderName)]
public class DeleteTests : BaseDeleteTests
{
    public DeleteTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
