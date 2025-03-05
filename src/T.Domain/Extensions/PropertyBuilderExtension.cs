using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace T.Domain.Extensions;

public static class PropertyBuilderExtension {

    public static PropertyBuilder<DateTimeOffset> HasDateConversion(this PropertyBuilder<DateTimeOffset> builder) {
        return builder.HasConversion(o => o.ToUnixTimeMilliseconds(), o => DateTimeOffset.FromUnixTimeMilliseconds(o));
    }

    public static PropertyBuilder<DateTimeOffset?> HasDateConversion(this PropertyBuilder<DateTimeOffset?> builder) {
        return builder.HasConversion(o => o.HasValue ? o.Value.ToUnixTimeMilliseconds() : -1,
                                     o => o >= 0 ? DateTimeOffset.FromUnixTimeMilliseconds(o) : null);
    }

    public static PropertyBuilder<decimal> HasQuantityPrecision(this PropertyBuilder<decimal> builder) {
        return builder.HasPrecision(28, 6);
    }

    public static PropertyBuilder<decimal> HasCurrencyPrecision(this PropertyBuilder<decimal> builder) {
        return builder.HasPrecision(28, 2);
    }
}
