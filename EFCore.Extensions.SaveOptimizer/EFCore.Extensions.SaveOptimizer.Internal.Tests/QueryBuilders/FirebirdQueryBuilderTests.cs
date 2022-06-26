using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Net6.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class FirebirdQueryBuilderTests : BaseQueryBuilderTests
{
    public FirebirdQueryBuilderTests()
        : base(new FirebirdCompiler(), () => new FirebirdQueryBuilder())
    {
    }
}
