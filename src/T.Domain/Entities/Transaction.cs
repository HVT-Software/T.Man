#region

using T.Domain.Common.Interfaces;
using T.Domain.Enums;

#endregion

namespace T.Domain.Entities;

public class Transaction : IEntity {
    public Guid             UserId      { get; set; }
    public Guid             MerchantId  { get; set; }
    public Guid?             CategoryId  { get; set; }
    public ETransactionType Type        { get; set; }
    public decimal          Amount      { get; set; }
    public string           Description { get; set; } = string.Empty;
    public DateTimeOffset   Date        { get; set; }
    public DateTimeOffset   CreatedAt   { get; set; }

    public virtual Category? Category  { get; set; }
    public virtual User?     User      { get; set; }
    public         Guid      Id        { get; set; }
    public         bool      IsDeleted { get; set; }
}
