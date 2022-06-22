using EFCore.Extensions.SaveOptimizer.Wrappers;

namespace EFCore.Extensions.SaveOptimizer.Resolvers;

public interface ICompilerWrapperResolver
{
    ICompilerWrapper Resolve(string providerName);
}
