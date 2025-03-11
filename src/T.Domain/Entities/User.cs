using T.Domain.Common.Interfaces;

namespace T.Domain.Entities;

public class User : IEntity {
    public Guid Id { get; set; }
    public Guid? RoleId { get; set; }
    public Guid MerchantId { get; set; }

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }

    public bool IsAdmin { get; set; }
    public bool IsSystem { get; set; }
    public bool IsDelete { get; set; }

    public long LastSession { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public virtual Role? Role { get; set; }
    public virtual Merchant? Merchant { get; set; }
}
