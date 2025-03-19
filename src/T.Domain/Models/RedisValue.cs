namespace T.Domain.Models;

public class RedisValue<T> {
    public T?        Value  { get; set; }
    public TimeSpan? Expiry { get; set; }

    public bool HasValue { get => Value != null; }
}
