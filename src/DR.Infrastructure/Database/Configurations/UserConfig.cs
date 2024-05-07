using DR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DR.Infrastructure.Database.Configurations;

internal class UserConfig : IEntityTypeConfiguration<User> {

    public void Configure(EntityTypeBuilder<User> builder) {
        builder.ToTable(nameof(User));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id);
        builder.Property(o => o.RoleId);

        builder.Property(o => o.Username).IsRequired();
        builder.Property(o => o.Password).IsRequired();

        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.Address).HasMaxLength(255);

        // fk
        builder.HasOne(o => o.Role).WithMany(o => o.Users).HasForeignKey(o => o.RoleId);

        // seed data

        builder.HasData(new User {
            Id = Guid.Parse("dec5aee5-12e1-4b61-8d3f-ad5d5235e6cd"),
            Name = "Admin",
            Username = "admin",
            Password = "",
            Phone = "",
            Address = "Thanh An, Hớn Quản, Bình Phước",
            IsActive = true,
            IsAdmin = true,
            IsDeleted = false,
            IsSystem = true,
        });
    }
}
