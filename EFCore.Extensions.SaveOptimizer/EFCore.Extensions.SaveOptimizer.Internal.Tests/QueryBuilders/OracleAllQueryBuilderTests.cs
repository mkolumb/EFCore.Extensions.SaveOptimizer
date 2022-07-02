using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class OracleAllQueryBuilderTests : BaseQueryBuilderTests
{
    public OracleAllQueryBuilderTests()
        : base(new OracleCompiler(), () => new OracleAllQueryBuilder(null))
    {
    }
}
