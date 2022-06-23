using EFCore.Extensions.SaveOptimizer.Shared.Tests;

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Tests;

public class UpdateTests : BaseUpdateTests
{
    public UpdateTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}