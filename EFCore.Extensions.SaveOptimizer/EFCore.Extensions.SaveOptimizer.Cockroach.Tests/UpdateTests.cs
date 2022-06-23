using EFCore.Extensions.SaveOptimizer.Shared.Tests;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

public class UpdateTests : BaseUpdateTests
{
    public UpdateTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}