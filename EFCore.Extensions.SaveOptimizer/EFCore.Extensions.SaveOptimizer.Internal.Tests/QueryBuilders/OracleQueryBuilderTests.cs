using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class OracleQueryBuilderTests : BaseQueryBuilderTests
{
    public OracleQueryBuilderTests()
        : base(new OracleCompiler(), () => new OracleQueryBuilder())
    {
    }
}
