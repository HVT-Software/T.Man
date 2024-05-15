using DR.Domain.Common;
using DR.Domain.Common.Interfaces;
using DR.Domain.Enums;

namespace DR.Domain.Entities;

public class Attachment : BaseAudit, IEntity {
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public FileType Type { get; set; }
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
}
