using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Internal.Wrappers;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryTranslatorService
{
    QueryDataModel? Translate(IDataContextModelWrapper context, EntityEntry entry);
}
