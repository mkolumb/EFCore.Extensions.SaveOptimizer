using EFCore.Extensions.SaveOptimizer.Internal.QueryBuilders;
using SqlKata.Compilers;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Internal.Tests.QueryBuilders;

public class MySqlQueryBuilderTests : BaseQueryBuilderTests
{
    public MySqlQueryBuilderTests()
        : base(new MySqlCompiler(), () => new MySqlQueryBuilder(null))
    {
    }
}
