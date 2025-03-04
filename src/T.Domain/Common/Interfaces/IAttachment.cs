using T.Domain.Common.Models;

namespace T.Domain.Common.Interfaces;

public interface IAttachment {
    public Guid? Id { get; set; }
    public List<AttachmentDto> Attachments { get; set; }
}
