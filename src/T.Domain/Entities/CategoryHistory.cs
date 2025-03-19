namespace T.Domain.Entities;

public class CategoryHistory {
    public Guid           Id           { get; set; }
    public Guid           CategoryId   { get; set; }
    public DateTimeOffset Month        { get; set; }
    public decimal        BudgetAmount { get; set; }
    public decimal        UsedAmount   { get; set; }
    public DateTimeOffset CreatedAt    { get; set; }

    public virtual Category? Category { get; set; }
}
