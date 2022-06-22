using EFCore.Extensions.SaveOptimizer.Shared.Tests;

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

// ReSharper disable once UnusedMember.Global
public class InsertTests : BaseInsertTests
{
    public InsertTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
