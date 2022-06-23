using System.Collections;
using System.Text;

namespace EFCore.Extensions.SaveOptimizer.Internal.Helpers;

public class SerializationHelper
{
    private static readonly Type[] NumberTypes =
    {
        typeof(int), typeof(long), typeof(decimal), typeof(double), typeof(float), typeof(short), typeof(ushort),
        typeof(ulong)
    };

    public static string Serialize(string key, object? value)
    {
        if (value == null)
        {
            return $"{key}=NULL";
        }

        if (NumberTypes.Contains(value.GetType()))
        {
            return $"{key}={value:G}";
        }

        switch (value)
        {
            case string:
                return $"{key}={value}";
            case byte[] bytes:
                return $"{key}={Convert.ToBase64String(bytes)}";
            case DateTimeOffset dateOffset:
                return $"{key}={dateOffset:O}";
            case DateTime date:
                return $"{key}={date:O}";
            case IEnumerable enumerable:
                {
                    StringBuilder builder = new();
                    builder.Append(key);
                    var i = 0;
                    foreach (var item in enumerable)
                    {
                        builder.Append(Serialize(i.ToString(), item));
                        i++;
                    }

                    return builder.ToString();
                }
            default:
                return $"{key}={value}";
        }
    }
}
