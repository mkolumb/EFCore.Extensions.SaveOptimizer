namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class SqlValueModel : IEquatable<SqlValueModel>
{
    public object? Value { get; }

    public PropertyTypeModel PropertyTypeModel { get; }

    public SqlValueModel(object? value, PropertyTypeModel propertyTypeModel)
    {
        Value = value;
        PropertyTypeModel = propertyTypeModel;
    }

    public bool Equals(SqlValueModel? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Equals(Value, other.Value) && Equals(PropertyTypeModel, other.PropertyTypeModel);
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

        return obj.GetType() == GetType() && Equals((SqlValueModel)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Value, PropertyTypeModel);
}
