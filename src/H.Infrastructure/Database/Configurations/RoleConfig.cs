using H.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace H.Infrastructure.Database.Configurations;

internal class RoleConfig : IEntityTypeConfiguration<Role> {

    public void Configure(EntityTypeBuilder<Role> builder) {
        builder.ToTable(nameof(Role));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasMaxLength(32);

        builder.Property(o => o.Code).HasMaxLength(50).IsRequired();
        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.SearchName).HasMaxLength(255).IsRequired();
        builder.Property(o => o.CreatedDate).HasConversion(o => o.ToUnixTimeMilliseconds(), o => DateTimeOffset.FromUnixTimeMilliseconds(o));

        // fk
        builder.HasMany(o => o.Users).WithOne(o => o.Role).HasForeignKey(o => o.RoleId);
        builder.HasMany(o => o.RolePermissions).WithOne(o => o.Role).HasForeignKey(o => o.RoleId);
    }
}
