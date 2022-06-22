using EFCore.Extensions.SaveOptimizer.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Resolvers;

public interface IDataContextModelWrapperResolver
{
    IDataContextModelWrapper Resolve<TContext>()
        where TContext : DbContext;
}
