using System.Collections;
using System.Text;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Internal.Helpers;

public class SerializationHelper
{
    private static readonly Type[] NumberTypes =
    {
        typeof(int), typeof(long), typeof(decimal), typeof(double), typeof(float), typeof(short), typeof(ushort),
        typeof(ulong)
    };

    public static string Serialize(string key, object? value) => $"{key}={Serialize(value)}";

    public static string Serialize(object? value)
    {
        while (true)
        {
            if (value == null)
            {
                return "NULL";
            }

            if (NumberTypes.Contains(value.GetType()))
            {
                return $"{value:G}";
            }

            switch (value)
            {
                case SqlValueModel sqlValue:
                    value = sqlValue.Value;
                    continue;
                case string:
                    return $"{value}";
                case byte[] bytes:
                    return $"{Convert.ToBase64String(bytes)}";
                case DateTimeOffset dateOffset:
                    return $"{dateOffset:O}";
                case DateTime date:
                    return $"{date:O}";
                case IEnumerable enumerable:
                    {
                        StringBuilder builder = new();
                        builder.Append('[');
                        var i = 0;
                        foreach (var item in enumerable)
                        {
                            builder.Append(Serialize(i.ToString(), item) + ",");
                            i++;
                        }
                        builder.Append(']');
                        return builder.ToString();
                    }
                default:
                    return $"{value}";
            }
        }
    }
}
