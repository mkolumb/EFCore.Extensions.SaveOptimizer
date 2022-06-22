namespace EFCore.Extensions.SaveOptimizer.Wrappers
{
    public interface IDataContextModelWrapper
    {
        string GetTableName<TEntity>();

        string? GetSchema<TEntity>();

        string GetColumn<TEntity>(string propertyName);
    }
}
