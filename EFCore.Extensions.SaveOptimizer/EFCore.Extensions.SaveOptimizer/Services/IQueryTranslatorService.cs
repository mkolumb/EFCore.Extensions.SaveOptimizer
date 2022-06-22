using EFCore.Extensions.SaveOptimizer.Models;
using EFCore.Extensions.SaveOptimizer.Wrappers;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Extensions.SaveOptimizer.Services;

public interface IQueryTranslatorService
{
    QueryDataModel? Translate(IDataContextModelWrapper context, EntityEntry entry);
}
