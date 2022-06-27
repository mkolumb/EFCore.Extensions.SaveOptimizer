using EFCore.Extensions.SaveOptimizer.Internal.Enums;

namespace EFCore.Extensions.SaveOptimizer.Internal.Configuration;

public class QueryBuilderConfiguration
{
    public CaseType CaseType { get; set; } = CaseType.Normal;
}
