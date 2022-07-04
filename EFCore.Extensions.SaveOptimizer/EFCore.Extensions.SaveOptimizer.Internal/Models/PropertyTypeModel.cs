using System.Data;
using System.Data.Common;

namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class PropertyTypeModel : IEquatable<PropertyTypeModel>
{
    public string ColumnName { get; }

    public string? ColumnType { get; }

    public Func<IDbCommand, string, object?, DbParameter> ParameterResolver { get; }

    public string Signature { get; }

    public PropertyTypeModel(string columnName,
        string? columnType,
        Func<IDbCommand, string, object?, DbParameter> parameterResolver,
        string signature)
    {
        ColumnName = columnName;
        ColumnType = columnType;
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

        return ColumnName == other.ColumnName && ColumnType == other.ColumnType && Signature == other.Signature;
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

    public override int GetHashCode() => HashCode.Combine(ColumnName, ColumnType, Signature);
}
