using System.Collections.Immutable;
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

    public static ImmutableArray<ImmutableArray<T>> ToDistinctChunks<T>(this IEnumerable<T> source, int chunkSize)
        where T : IEquatable<T> =>
        source
            .OrderBy(x => x)
            .Distinct()
            .Select((s, i) => new { Value = s, Index = i })
            .GroupBy(x => x.Index / chunkSize)
            .Select(grp => grp.Select(x => x.Value).OrderBy(x => x).ToImmutableArray())
            .ToImmutableArray();

    public static ImmutableArray<ImmutableArray<T>> ToChunks<T>(this IEnumerable<T> source, int chunkSize) =>
        source
            .Select((s, i) => new { Value = s, Index = i })
            .GroupBy(x => x.Index / chunkSize)
            .Select(grp => grp.Select(x => x.Value).ToImmutableArray())
            .ToImmutableArray();
}
