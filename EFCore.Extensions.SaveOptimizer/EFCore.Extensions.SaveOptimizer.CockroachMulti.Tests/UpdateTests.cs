using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.CockroachMulti.Tests;

[Collection("CockroachMulti")]
public class UpdateTests : BaseUpdateTests
{
    public UpdateTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
