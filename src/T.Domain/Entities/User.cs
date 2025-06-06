﻿#region

using T.Domain.Common.Interfaces;

#endregion

namespace T.Domain.Entities;

public class User : IEntity {
    public Guid? RoleId     { get; set; }
    public Guid  MerchantId { get; set; }

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string  Name    { get; set; } = null!;
    public string? Phone   { get; set; }
    public string? Email   { get; set; }
    public string? Address { get; set; }
    public string? Avatar  { get; set; }

    public bool    IsActive { get; set; }
    public bool    IsAdmin  { get; set; }
    public bool    IsSystem { get; set; }
    public string? Provider { get; set; }

    public long           LastSession { get; set; }
    public DateTimeOffset CreatedAt   { get; set; } = DateTimeOffset.UtcNow;

    public virtual Role?     Role     { get; set; }
    public virtual Merchant? Merchant { get; set; }

    public virtual ICollection<Transaction>?  Transactions  { get; set; }
    public virtual ICollection<Debt>?         Debit         { get; set; }
    public virtual ICollection<Notification>? Notifications { get; set; }
    public         Guid                       Id            { get; set; }
    public         bool                       IsDeleted     { get; set; }
}
