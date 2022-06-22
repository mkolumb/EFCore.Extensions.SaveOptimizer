using EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Load.Tests.Helpers;

public static class DataHelper
{
    public static EntityEntry[] ResolveData(TestDataContext context)
    {
        for (var i = 0; i < 100000; i++)
        {
            PerformanceEntity newEntity = new()
            {
                PrimaryKeyValue = Guid.NewGuid().ToString(),
                UpdatedDate = DateTime.Now.AddDays(1),
                CreatedDate = DateTime.Now.AddDays(3)
            };

            context.Add(newEntity);

            if (i % 2 == 0)
            {
                context.Entry(newEntity).State = EntityState.Deleted;
            }

            if (i % 3 == 0)
            {
                context.Entry(newEntity).State = EntityState.Added;
            }

            if (i % 6 == 0)
            {
                context.Entry(newEntity).State = EntityState.Modified;
            }
        }

        return context.ChangeTracker.Entries().ToArray();
    }
}
