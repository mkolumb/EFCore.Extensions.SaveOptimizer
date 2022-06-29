using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class SqliteQueryBuilderTests : BaseQueryBuilderTests
{
    public SqliteQueryBuilderTests()
        : base(new SqliteCompiler(), () => new SqliteQueryBuilder())
    {
    }
}
