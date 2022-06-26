using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Net6.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class SqlServerQueryBuilderTests : BaseQueryBuilderTests
{
    public SqlServerQueryBuilderTests()
        : base(new SqlServerCompiler { UseLegacyPagination = false }, () => new SqlServerQueryBuilder())
    {
    }
}
