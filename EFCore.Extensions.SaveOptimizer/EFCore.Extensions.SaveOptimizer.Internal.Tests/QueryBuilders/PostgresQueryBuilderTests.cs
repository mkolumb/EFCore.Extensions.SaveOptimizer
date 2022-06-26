using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class PostgresQueryBuilderTests : BaseQueryBuilderTests
{
    public PostgresQueryBuilderTests()
        : base(new PostgresCompiler(), () => new PostgresQueryBuilder())
    {
    }
}
