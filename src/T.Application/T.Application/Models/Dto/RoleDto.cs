#region

using System.Diagnostics.CodeAnalysis;
using T.Domain.Enums.Systems;

#endregion

namespace T.Application.Models.Dto;

public class RoleDto {
    public Guid           Id        { get; set; }
    public string?        Code      { get; set; }
    public string         Name      { get; set; } = string.Empty;
    public List<EAction>  Actions   { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }

    [return: NotNullIfNotNull(nameof(entity))]
    public static RoleDto? FromEntity(Role? entity, List<EAction>? actions) {
        if (entity == null) { return null; }

        return new RoleDto {
            Id        = entity.Id,
            Code      = entity.Code,
            Name      = entity.Name,
            Actions   = actions ?? [],
            CreatedAt = entity.CreateAt,
        };
    }
}
