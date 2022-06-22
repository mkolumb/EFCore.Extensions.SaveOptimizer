namespace EFCore.Extensions.SaveOptimizer.Wrappers
{
    public interface IDataContextModelWrapper
    {
        string GetTableName(Type entityType);

        string? GetSchema(Type entityType);

        string GetColumn(Type entityType, string propertyName);
    }
}
