using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;

namespace EFCore.Extensions.SaveOptimizer.Internal.Resolvers;

public interface ICompilerWrapperResolver
{
    ICompilerWrapper Resolve(string providerName);
}
