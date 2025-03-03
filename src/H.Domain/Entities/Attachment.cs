using H.Domain.Common;
using H.Domain.Common.Interfaces;
using H.Domain.Enums;

namespace H.Domain.Entities;

public class Attachment : BaseAudit, IEntity {
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public FileType Type { get; set; }
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
}
