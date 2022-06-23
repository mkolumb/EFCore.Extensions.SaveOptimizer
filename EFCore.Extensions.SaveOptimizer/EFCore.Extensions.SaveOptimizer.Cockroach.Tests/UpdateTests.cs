using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

[Collection("Cockroach")]
public class UpdateTests : BaseUpdateTests
{
    public UpdateTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
