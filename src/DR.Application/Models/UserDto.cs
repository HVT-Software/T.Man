using System.Diagnostics.CodeAnalysis;
using DR.Domain.Extentions;

namespace DR.Application.Models;

public class UserDto {
    public Guid? Id { get; set; }
    public string? Username { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }

    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsDeleted { get; set; }

    public RoleDto? Role { get; set; }

    [return: NotNullIfNotNull(nameof(entity))]
    public static UserDto? FromEntity(User? entity, Role? roleEntity = null) {
        if (entity == null) return default;
        entity.Role ??= roleEntity;

        return new UserDto {
            Id = entity.Id,
            Username = entity.Username,
            Name = entity.Name,
            Phone = entity.Phone,
            Address = entity.Address,
            IsAdmin = entity.IsAdmin,
            IsActive = entity.IsActive,
            Role = RoleDto.FromEntity(entity.Role),
        };
    }

    public User ToEntity() {
        return new User {
            Id = NGuidExtention.NewIfNull(Id),
            Username = Username ?? string.Empty,
            Name = Name,
            Phone = Phone,
            Address = Address,
            IsActive = IsActive,
            IsAdmin = IsAdmin,
            IsDeleted = IsDeleted,
        };
    }
}
