using System.Diagnostics.CodeAnalysis;
using T.Domain.Entities;
using T.Domain.Enums;

namespace T.Domain.Common.Models;

public class AttachmentDto {
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public FileType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;

    [return: NotNullIfNotNull(nameof(entity))]
    public static AttachmentDto? FromEntity(Attachment? entity) {
        if (entity == null) return default;
        return new AttachmentDto {
            Id = entity.Id,
            ParentId = entity.ParentId,
            Name = entity.Name,
            Path = entity.Path,
            Type = entity.Type,
        };
    }
}
