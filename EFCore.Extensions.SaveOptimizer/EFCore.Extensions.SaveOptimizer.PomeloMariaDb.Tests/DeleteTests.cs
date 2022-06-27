using EFCore.Extensions.SaveOptimizer.Shared.Tests;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Tests;

[Collection("PomeloMariaDb")]
public class DeleteTests : BaseDeleteTests
{
    public DeleteTests() : base(WrapperResolver.ContextWrapperResolver)
    {
    }
}
