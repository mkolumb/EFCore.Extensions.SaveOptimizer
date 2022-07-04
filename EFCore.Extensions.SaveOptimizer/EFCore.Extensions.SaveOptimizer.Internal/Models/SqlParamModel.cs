namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class SqlParamModel : IEquatable<SqlParamModel>
{
    public string Key { get; }

    public string CastValue { get; }

    public SqlValueModel SqlValueModel { get; }

    public SqlParamModel(string key, string castValue, SqlValueModel sqlValueModel)
    {
        Key = key;
        CastValue = castValue;
        SqlValueModel = sqlValueModel;
    }

    public bool Equals(SqlParamModel? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Key == other.Key && CastValue == other.CastValue && SqlValueModel.Equals(other.SqlValueModel);
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

        return obj.GetType() == GetType() && Equals((SqlParamModel)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Key, CastValue, SqlValueModel);
}
