namespace T.Infrastructure.Database.Configurations;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification));

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Content).IsRequired();
        builder.Property(n => n.Type).IsRequired();
        builder.Property(n => n.SentAt).IsRequired();
        builder.Property(n => n.IsRead).IsRequired();

        builder.HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserId);
    }
}
