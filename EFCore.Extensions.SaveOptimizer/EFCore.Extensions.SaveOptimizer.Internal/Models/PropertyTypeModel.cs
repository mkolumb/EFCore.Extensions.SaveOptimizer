using System.Data.Common;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class PropertyTypeModel : IEquatable<PropertyTypeModel>
{
    public string ColumnName { get; }

    public Func<DbCommand, string, object?, DbParameter> ParameterResolver { get; }

    public string Signature { get; }

    public PropertyTypeModel(string columnName,
        Func<DbCommand, string, object?, DbParameter> parameterResolver,
        string signature)
    {
        ColumnName = columnName;
        ParameterResolver = parameterResolver;
        Signature = signature;
    }

    public bool Equals(PropertyTypeModel? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ColumnName == other.ColumnName && Signature == other.Signature;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((PropertyTypeModel)obj);
    }

    public override int GetHashCode() => HashCode.Combine(ColumnName, Signature);
}
