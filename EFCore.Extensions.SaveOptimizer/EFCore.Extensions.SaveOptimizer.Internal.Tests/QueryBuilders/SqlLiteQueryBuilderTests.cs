using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class SqlLiteQueryBuilderTests : BaseQueryBuilderTests
{
    public SqlLiteQueryBuilderTests()
        : base(new SqliteCompiler(), () => new SqliteQueryBuilder())
    {
    }
}
