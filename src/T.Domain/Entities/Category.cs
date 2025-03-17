namespace T.Domain.Entities;

public class Category {
    public Guid Id { get; set; }
    public Guid MerchantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }

    public DateTimeOffset CreateAt { get; set; }
    public virtual Merchant? Merchant { get; set; }
}
