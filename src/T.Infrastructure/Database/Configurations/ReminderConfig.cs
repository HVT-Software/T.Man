namespace T.Infrastructure.Database.Configurations;

internal class ReminderConfig : IEntityTypeConfiguration<Reminder> {
    public void Configure(EntityTypeBuilder<Reminder> builder) {
        builder.ToTable(nameof(Reminder));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired();

        builder.Property(o => o.Title).IsRequired().HasMaxLength(255);
        builder.Property(o => o.Content).IsRequired();
        builder.Property(o => o.Email).IsRequired().HasMaxLength(255);
        builder.Property(o => o.SendDate).IsRequired();
        builder.Property(o => o.SendTime).IsRequired().HasMaxLength(10);
        builder.Property(o => o.Frequency).IsRequired();

        builder.Property(o => o.IsSent).IsRequired();
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.UpdatedAt);

        builder.HasOne(o => o.User).WithMany().HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.SetNull);
    }
}
