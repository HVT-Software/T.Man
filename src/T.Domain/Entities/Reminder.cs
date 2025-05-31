using T.Domain.Enums;

namespace T.Domain.Entities;

public class Reminder {
    public Guid           Id       { get; set; }
    public string         Title    { get; set; } = null!;
    public string         Content  { get; set; } = null!;
    public string         Email    { get; set; } = null!;
    public DateTimeOffset SendDate { get; set; }
    public string         SendTime { get; set; } = null!;

    public EReminderFrequency Frequency { get; set; }

    public bool            IsSent    { get; set; }
    public DateTimeOffset  CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    public         Guid? UserId { get; set; }
    public virtual User? User   { get; set; }
}
