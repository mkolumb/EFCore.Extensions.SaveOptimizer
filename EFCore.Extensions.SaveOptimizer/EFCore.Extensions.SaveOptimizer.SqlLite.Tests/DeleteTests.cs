using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.SqlLite.Tests;

[Collection("SqlLite")]
public class DeleteTests : BaseDeleteTests
{
    public DeleteTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
