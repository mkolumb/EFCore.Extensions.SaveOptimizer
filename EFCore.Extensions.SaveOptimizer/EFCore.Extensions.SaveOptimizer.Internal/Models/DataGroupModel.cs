namespace EFCore.Extensions.SaveOptimizer.Internal.Models;

public class DataGroupModel : IEquatable<DataGroupModel>
{
    public string Key { get; }

    public object? Value { get; }

    public HashSet<DataGroupModel> NestedItems { get; set; } = new();

    public DataGroupModel(string key, object? value)
    {
        Key = key;
        Value = value;
    }

    public bool Equals(DataGroupModel? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Key == other.Key && Equals(Value, other.Value);
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

        return obj.GetType() == GetType() && Equals((DataGroupModel)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Key, Value);

    public static HashSet<DataGroupModel> CreateDataGroup(
        IReadOnlyCollection<QueryDataModel> items,
        IReadOnlyList<string> keys)
    {
        HashSet<DataGroupModel> data = new();

        foreach (QueryDataModel item in items)
        {
            for (var i = 0; i < keys.Count; i++)
            {
                HashSet<DataGroupModel> set = data;

                for (var j = 0; j <= i; j++)
                {
                    var key = keys[j];

                    var value = item.Data[key];

                    DataGroupModel? dataItem = new(key, value);

                    if (!set.Contains(dataItem))
                    {
                        set.Add(dataItem);
                    }

                    if (set.TryGetValue(dataItem, out dataItem))
                    {
                        set = dataItem.NestedItems;
                    }
                }
            }
        }

        return data;
    }
}
