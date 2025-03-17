using T.Domain.Enums;

namespace T.Domain.Entities;

public class Debt {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset ExpectedReturnDate { get; set; }
    public EDebtStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
