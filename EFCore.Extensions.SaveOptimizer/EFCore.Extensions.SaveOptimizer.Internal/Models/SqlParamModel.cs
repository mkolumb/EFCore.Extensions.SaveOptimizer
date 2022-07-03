namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class SqlParamModel : IEquatable<SqlParamModel>
{
    public string Key { get; }

    public SqlValueModel SqlValueModel { get; }

    public SqlParamModel(string key, SqlValueModel sqlValueModel)
    {
        Key = key;
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

        return Key == other.Key && SqlValueModel.Equals(other.SqlValueModel);
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

    public override int GetHashCode() => HashCode.Combine(Key, SqlValueModel);
}
