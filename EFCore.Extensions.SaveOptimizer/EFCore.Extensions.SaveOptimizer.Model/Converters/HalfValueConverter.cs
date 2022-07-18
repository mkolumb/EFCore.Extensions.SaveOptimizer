using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.Extensions.SaveOptimizer.Model.Converters;

public class HalfValueConverter : ValueConverter<Half?, string?>
{
    public HalfValueConverter() : base(ConvertTo(), ConvertFrom())
    {
    }

    private static Expression<Func<string?, Half?>> ConvertFrom() => value => value != null ? Half.Parse(value) : null;

    private static Expression<Func<Half?, string?>> ConvertTo() =>
        value => value != null ? value.Value.ToString() : null;
}
