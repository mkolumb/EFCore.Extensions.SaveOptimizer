using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Wrappers
{
    public interface IDataContextModelWrapper
    {
        string GetTableName(Type entityType);

        string? GetSchema(Type entityType);

        PropertyTypeModel GetColumn(Type entityType, string propertyName);
    }
}
