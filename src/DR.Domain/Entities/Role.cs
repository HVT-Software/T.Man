using System.ComponentModel;
using DR.Domain.Common.Interfaces;

namespace DR.Domain.Entities;

public class Role : IEntity {
    public Guid Id { get; set; }

    [Description("Mã phân quyền")]
    public string Code { get; set; } = null!;

    [Description("Tên phân quyền")]
    public string Name { get; set; } = null!;

    public string SearchName { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedDate { get; } = DateTimeOffset.UtcNow;

    public virtual ICollection<User>? Users { get; set; }
    public virtual ICollection<RolePermission>? RolePermissions { get; set; }
}
