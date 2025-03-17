namespace T.Domain.Entities;

public class Merchant {
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string SearchName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public virtual ICollection<User>? Users { get; set; }
    public virtual ICollection<Category>? Categories { get; set; }
}
