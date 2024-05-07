using DR.Domain.Common;

namespace DR.Domain.Entities;

public class Permission : IEntity, ICloneable {
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string ClaimName { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public bool IsClaim { get; set; }
    public int OrderIndex { get; set; }
    public virtual ICollection<RolePermission>? RolePermissions { get; set; }

    public object Clone() {
        return new Permission {
            Id = Id,
            ParentId = ParentId,
            ClaimName = ClaimName,
            DisplayName = DisplayName,
            IsDefault = IsDefault,
            IsActive = IsActive,
            IsClaim = IsClaim,
            OrderIndex = OrderIndex,
        };
    }
}
