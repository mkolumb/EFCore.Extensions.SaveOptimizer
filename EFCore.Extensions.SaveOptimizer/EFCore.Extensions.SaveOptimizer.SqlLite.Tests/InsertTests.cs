using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Tests;

[Collection("SqlLite")]
public class InsertTests : BaseInsertTests
{
    public InsertTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
