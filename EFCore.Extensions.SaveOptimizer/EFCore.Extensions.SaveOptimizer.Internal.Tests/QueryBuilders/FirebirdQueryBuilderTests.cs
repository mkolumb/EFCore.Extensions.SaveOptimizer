using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class FirebirdQueryBuilderTests : BaseQueryBuilderTests
{
    public FirebirdQueryBuilderTests()
        : base(new FirebirdCompiler(), () => new FirebirdQueryBuilder())
    {
    }
}
