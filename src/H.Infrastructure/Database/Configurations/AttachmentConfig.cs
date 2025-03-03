using H.Domain.Common;
using H.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace H.Infrastructure.Database.Configurations;

internal class AttachmentConfig : IEntityTypeConfiguration<Attachment> {

    public void Configure(EntityTypeBuilder<Attachment> builder) {
        builder.ToTable(nameof(Attachment));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.Path).HasMaxLength(2000).IsRequired();
        builder.AddAuditConfig();
        // index

        builder.HasIndex(o => new { o.Type, o.ParentId });
    }
}
