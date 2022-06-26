using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class SqlServerQueryBuilderTests : BaseQueryBuilderTests
{
    public SqlServerQueryBuilderTests()
        : base(new SqlServerCompiler { UseLegacyPagination = false }, () => new SqlServerQueryBuilder())
    {
    }
}
