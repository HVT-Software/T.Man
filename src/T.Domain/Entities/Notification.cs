using T.Domain.Common.Interfaces;
using T.Domain.Enums;

namespace T.Domain.Entities;

public class Notification : IEntity {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public ENotificationType Type { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsRead { get; set; }

    public virtual User? User { get; set; }
}
