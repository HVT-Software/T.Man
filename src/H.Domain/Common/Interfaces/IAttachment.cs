using H.Domain.Common.Models;

namespace H.Domain.Common.Interfaces;

public interface IAttachment {
    public Guid? Id { get; set; }
    public List<AttachmentDto> Attachments { get; set; }
}
