#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace T.Domain.Common;

public abstract class BaseAudit {
    public bool  IsDeleted { get; set; }
    public Guid  CreateBy  { get; set; }
    public Guid? UpdateBy  { get; set; }
    public Guid? DeleteBy  { get; set; }

    public DateTimeOffset  CreateAt { get; set; }
    public DateTimeOffset? UpdateAt { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
}


public static class AuditConfig {
    public static void AddAuditConfig<T>(this EntityTypeBuilder<T> builder) where T : BaseAudit {
        builder.Property(o => o.IsDeleted).HasDefaultValue(false);

        builder.Property(o => o.CreateAt)
            .HasConversion(o => o.ToUnixTimeMilliseconds(), o => DateTimeOffset.FromUnixTimeMilliseconds(o))
            .IsRequired();
        builder.Property(o => o.UpdateAt)
            .HasConversion(
                o => o.HasValue ? o.Value.ToUnixTimeMilliseconds() : -1,
                o => o >= 0 ? DateTimeOffset.FromUnixTimeMilliseconds(o) : null);
        builder.Property(o => o.DeleteAt)
            .HasConversion(
                o => o.HasValue ? o.Value.ToUnixTimeMilliseconds() : -1,
                o => o >= 0 ? DateTimeOffset.FromUnixTimeMilliseconds(o) : null);
    }
}
