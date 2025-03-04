using T.Domain.Common;
using T.Domain.Common.Interfaces;
using T.Domain.Enums;

namespace T.Domain.Entities;

public class Attachment : BaseAudit, IEntity {
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public FileType Type { get; set; }
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
}
