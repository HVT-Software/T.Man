using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using T.Domain.Entities;

namespace T.Infrastructure.Database.Configurations;

internal class RolePermissionConfig : IEntityTypeConfiguration<RolePermission> {

    public void Configure(EntityTypeBuilder<RolePermission> builder) {
        builder.ToTable(nameof(RolePermission));

        builder.HasKey(o => new { o.RoleId, o.PermissionId });
        builder.Property(o => o.RoleId).HasMaxLength(32);
        builder.Property(o => o.PermissionId).HasMaxLength(32);

        // fk
        builder.HasOne(o => o.Role).WithMany(o => o.RolePermissions).HasForeignKey(o => o.RoleId);
        builder.HasOne(o => o.Permission).WithMany(o => o.RolePermissions).HasForeignKey(o => o.PermissionId);
    }
}
