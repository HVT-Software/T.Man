namespace T.Infrastructure.Database.Configurations;

internal class RoleActionConfig : IEntityTypeConfiguration<RoleAction> {

    public void Configure(EntityTypeBuilder<RoleAction> builder) {
        builder.ToTable(nameof(RoleAction));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.RoleId).HasMaxLength(36);

        builder.Property(o => o.RoleId).HasMaxLength(32).IsRequired();
        builder.Property(o => o.Key).HasMaxLength(50);

        // fk
        builder.HasOne(o => o.Role).WithMany(o => o.RoleActions).HasForeignKey(o => o.RoleId);
    }
}
