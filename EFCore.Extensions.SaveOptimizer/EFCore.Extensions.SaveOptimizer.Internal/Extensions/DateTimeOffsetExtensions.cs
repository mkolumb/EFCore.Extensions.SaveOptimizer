namespace EFCore.Extensions.SaveOptimizer.Internal.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset ToDbOffset(this DateTimeOffset offset)
    {
        var date = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        var milliseconds = offset.ToUnixTimeMilliseconds();

        date = date.AddMilliseconds(milliseconds);

        date = date.AddMilliseconds(date.Millisecond * -1);

        return date;
    }
}
