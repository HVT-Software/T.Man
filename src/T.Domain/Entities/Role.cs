using T.Domain.Common.Interfaces;

namespace T.Domain.Entities;

public class Role : IEntity {
    public Guid Id { get; set; }
    public Guid MerchantId { get; set; }

    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public string SearchName { get; set; } = null!;
    public DateTimeOffset CreateAt { get; } = DateTimeOffset.UtcNow;

    public bool IsDelete { get; set; }

    public virtual ICollection<User>? Users { get; set; }
    public virtual ICollection<RoleAction>? RoleActions { get; set; }
}
