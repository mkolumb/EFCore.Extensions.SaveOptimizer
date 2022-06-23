using EFCore.Extensions.SaveOptimizer.Shared.Tests;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

public class DeleteTests : BaseDeleteTests
{
    public DeleteTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}