using EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Tests;

[Collection(Variables.ProviderName)]
public class UpdateTests : BaseUpdateTests
{
    public UpdateTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
