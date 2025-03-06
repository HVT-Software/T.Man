using T.Domain.Extensions;

namespace T.Infrastructure.Database.Configurations;

internal class RoleConfig : IEntityTypeConfiguration<Role> {

    public void Configure(EntityTypeBuilder<Role> builder) {
        builder.ToTable(nameof(Role));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasMaxLength(32);
        builder.Property(o => o.MerchantId).HasMaxLength(32).IsRequired();

        builder.Property(o => o.Code).HasMaxLength(50).IsRequired();
        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.SearchName).HasMaxLength(255).IsRequired();
        builder.Property(o => o.CreateAt).HasDateConversion().IsRequired();

        // index
        builder.HasIndex(o => o.MerchantId);
        builder.HasIndex(o => new { o.MerchantId, o.Code }).IsUnique();

        // fk
        builder.HasMany(o => o.Users).WithOne(o => o.Role).HasForeignKey(o => o.RoleId);
        builder.HasMany(o => o.RoleActions).WithOne(o => o.Role).HasForeignKey(o => o.RoleId);
    }
}
