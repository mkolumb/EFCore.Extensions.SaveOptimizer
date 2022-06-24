using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Postgres.Tests;

[Collection("Postgres")]
public class UpdateTests : BaseUpdateTests
{
    public UpdateTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
