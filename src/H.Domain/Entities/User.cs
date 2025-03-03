using System.ComponentModel;
using H.Domain.Common.Interfaces;

namespace H.Domain.Entities;

public class User : IEntity {
    public Guid Id { get; set; }

    [Description("Phân quyền")]
    public Guid? RoleId { get; set; }

    [Description("Tên đăng nhập")]
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Description("Tên người dùng")]
    public string Name { get; set; } = null!;

    [Description("Số điện thoại")]
    public string? Phone { get; set; }

    [Description("Địa chỉ")]
    public string? Address { get; set; }

    [Description("Trạng thái")]
    public bool IsActive { get; set; }

    public bool IsAdmin { get; set; }
    public bool IsSystem { get; set; }
    public bool IsDeleted { get; set; }

    public virtual Role? Role { get; set; }
}
