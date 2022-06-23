using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Cockroach.Tests;

[Collection("Cockroach")]
public class InsertTests : BaseInsertTests
{
    public InsertTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
