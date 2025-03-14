namespace T.Infrastructure.Database.Configurations;

internal class UserConfig : IEntityTypeConfiguration<User> {

    public void Configure(EntityTypeBuilder<User> builder) {
        builder.ToTable(nameof(User));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id);
        builder.Property(o => o.RoleId).HasMaxLength(36);
        builder.Property(o => o.MerchantId).HasMaxLength(36).IsRequired();

        builder.Property(o => o.Username).HasMaxLength(50).IsRequired();
        builder.Property(o => o.Password).HasMaxLength(500).IsRequired();
        builder.Property(o => o.Provider).HasMaxLength(50);

        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.Address).HasMaxLength(255);
        builder.Property(o => o.Phone).HasMaxLength(11);
        builder.Property(o => o.Email).HasMaxLength(255);
        builder.Property(o => o.Avatar).HasMaxLength(2000);

        // index
        builder.HasIndex(o => o.MerchantId);
        builder.HasIndex(o => new { o.MerchantId, o.Username }).IsUnique();

        // fk
        builder.HasOne(o => o.Role).WithMany(o => o.Users).HasForeignKey(o => o.RoleId);
        builder.HasOne(o => o.Merchant).WithMany(o => o.Users).HasForeignKey(o => o.MerchantId);
    }
}
