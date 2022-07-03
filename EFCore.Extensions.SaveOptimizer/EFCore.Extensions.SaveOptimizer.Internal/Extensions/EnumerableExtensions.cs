using System.Text;

namespace EFCore.Extensions.SaveOptimizer.Internal.Extensions;

public static class EnumerableExtensions
{
    public static string ToRepresentation<T>(this IEnumerable<T> source, Func<T, string?>? converter = null)
    {
        StringBuilder builder = new();

        var first = true;

        converter ??= arg => arg?.ToString();

        foreach (T item in source)
        {
            builder.Append(first ? converter(item) : $" | {converter(item)}");

            if (first)
            {
                first = false;
            }
        }

        return builder.ToString();
    }
}
