using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;

public static class DbWrapperExtensions
{
    public static void DisposeAll(this IEnumerable<DbContextWrapper> wrappers)
    {
        foreach (DbContextWrapper wrapper in wrappers)
        {
            try
            {
                wrapper.Dispose();
            }
            catch
            {
                // nothing
            }
        }
    }
}
