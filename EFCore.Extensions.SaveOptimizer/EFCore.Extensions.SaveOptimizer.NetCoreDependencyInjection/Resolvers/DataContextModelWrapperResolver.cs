using System.Collections.Concurrent;
using EFCore.Extensions.SaveOptimizer.Resolvers;
using EFCore.Extensions.SaveOptimizer.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Extensions.SaveOptimizer.NetCoreDependencyInjection.Resolvers;

public class DataContextModelWrapperResolver : IDataContextModelWrapperResolver
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<Type, IDataContextModelWrapper> _wrappers;

    public DataContextModelWrapperResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _wrappers = new ConcurrentDictionary<Type, IDataContextModelWrapper>();
    }

    public IDataContextModelWrapper Resolve<TContext>()
        where TContext : DbContext =>
        _wrappers.GetOrAdd(typeof(TContext),
            new DataContextModelWrapper<TContext>(_serviceProvider.GetRequiredService<TContext>));
}
