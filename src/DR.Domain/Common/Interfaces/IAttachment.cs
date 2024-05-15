using DR.Domain.Common.Models;

namespace DR.Domain.Common.Interfaces;

public interface IAttachment {
    public Guid? Id { get; set; }
    public List<AttachmentDto> Attachments { get; set; }
}
