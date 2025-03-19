#region

using T.Domain.Common.Interfaces;

#endregion

namespace T.Domain.Entities;

public class Role : IEntity {
    public Guid MerchantId { get; set; }

    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public string         SearchName { get; set; } = null!;
    public DateTimeOffset CreateAt   { get; }      = DateTimeOffset.UtcNow;

    public virtual ICollection<User>?       Users       { get; set; }
    public virtual ICollection<RoleAction>? RoleActions { get; set; }
    public         Guid                     Id          { get; set; }

    public bool IsDeleted { get; set; }
}
