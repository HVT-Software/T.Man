#region

using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace T.Application.Models.Dto;

public class UserDto {
    public Guid Id { get; set; }

    [Description("Username")]
    public string? Username { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
    public string? Password { get; set; }

    public bool IsActive { get; set; }

    public RoleDto? Role { get; set; }

    public bool IsAdmin { get; set; }

    [Description("CreatedAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [return: NotNullIfNotNull(nameof(entity))]
    public static UserDto? FromEntity(User? entity) {
        if (entity == null) { return null; }

        return new UserDto {
            Id        = entity.Id,
            Username  = entity.Username,
            IsActive  = entity.IsActive,
            IsAdmin   = entity.IsAdmin,
            Role      = RoleDto.FromEntity(entity.Role, []),
            CreatedAt = entity.CreatedAt,
        };
    }
}
