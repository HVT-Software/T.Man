using DR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DR.Infrastructure.Database.Configurations;

internal class PermissionConfig : IEntityTypeConfiguration<Permission> {

    public void Configure(EntityTypeBuilder<Permission> builder) {
        builder.ToTable(nameof(Permission));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasMaxLength(32);
        builder.Property(o => o.ParentId).HasMaxLength(32);

        builder.Property(o => o.ClaimName).HasMaxLength(50).IsRequired();
        builder.Property(o => o.DisplayName).HasMaxLength(50).IsRequired();

        // fk
        builder.HasMany(o => o.RolePermissions).WithOne(o => o.Permission).HasForeignKey(o => o.PermissionId);
        var index = 0;

        // Quản lý dự án
        builder.HasData(new Permission {
            Id = Guid.Parse("01f543fd-521f-41c7-83b3-00253996dd69"),
            ParentId = null,
            ClaimName = "DR.Kaban",
            DisplayName = "Quản lý dự án",
            IsDefault = true,
            IsActive = true,
            IsClaim = true,
            OrderIndex = index++,
        });

        // Quản lý công cụ
        builder.HasData(new Permission {
            Id = Guid.Parse("4ff1aa66-fc29-4e06-becb-6e307e6aa09a"),
            ParentId = null,
            ClaimName = "DR.DevTools",
            DisplayName = "Công cụ",
            IsDefault = true,
            IsActive = true,
            IsClaim = true,
            OrderIndex = index++,
        });

        // Quản lý người dùng

        builder.HasData(new Permission {
            Id = Guid.Parse("cc91c9c4-5845-407d-867b-0c1453f2b852"),
            ParentId = null,
            ClaimName = "DR.User",
            DisplayName = "Quản lý người dùng",
            IsDefault = false,
            IsActive = true,
            IsClaim = true,
            OrderIndex = index++,
        });

        builder.HasData(new Permission {
            Id = Guid.Parse("31f07c51-7067-4e96-9f44-de6a02818513"),
            ParentId = Guid.Parse("de5ffa57-021d-4768-b361-894828259350"),
            ClaimName = "DR.User.Password",
            DisplayName = "Quản lý mật khẩu",
            IsDefault = false,
            IsActive = true,
            IsClaim = true,
            OrderIndex = index++,
        });

        builder.HasData(new Permission {
            Id = Guid.Parse("8ad5baf8-b7f6-433c-94e7-87ca45728945"),
            ParentId = Guid.Parse("cc91c9c4-5845-407d-867b-0c1453f2b852"),
            ClaimName = "DR.User.Edit",
            DisplayName = "Cập nhật người dùng",
            IsDefault = false,
            IsActive = true,
            IsClaim = true,
            OrderIndex = index++,
        });
    }
}
