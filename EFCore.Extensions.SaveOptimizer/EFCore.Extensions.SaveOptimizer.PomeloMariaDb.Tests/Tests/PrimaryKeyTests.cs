using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Tests.Tests;

[EntityCollection(Variables.ProviderName, typeof(AutoIncrementPrimaryKeyEntity))]
public class PrimaryKeyTests : BasePrimaryKeyTests
{
    public PrimaryKeyTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
