namespace T.Domain.Extensions;

public static class DateTimeExtension {
    public static (DateTimeOffset? From, DateTimeOffset? To) ToDateTimeOffsetRange(this List<long>? timestamps) {
        if (timestamps == null || timestamps.Count == 0) { return(null, null); }

        var             from = DateTimeOffset.FromUnixTimeMilliseconds(timestamps.First());
        DateTimeOffset? to   = timestamps.Count == 1 ? null : DateTimeOffset.FromUnixTimeMilliseconds(timestamps.Last());

        return(from, to);
    }

    public static (DateTimeOffset? From, DateTimeOffset? To) GetMonthRange(this DateTimeOffset dateTimeOffset) {
        var startOfMonth = new DateTimeOffset(
            dateTimeOffset.Year,
            dateTimeOffset.Month,
            1,
            0,
            0,
            0,
            dateTimeOffset.Offset);
        DateTimeOffset endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        return(startOfMonth, endOfMonth);
    }
}
