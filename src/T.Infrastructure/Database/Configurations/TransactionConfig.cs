#region

using T.Domain.Extensions;

#endregion

namespace T.Infrastructure.Database.Configurations;

public class TransactionConfig : IEntityTypeConfiguration<Transaction> {
    public void Configure(EntityTypeBuilder<Transaction> builder) {
        builder.ToTable(nameof(Transaction));

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount).HasCurrencyPrecision().IsRequired();
        builder.Property(t => t.Date).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.Type).IsRequired();

        builder.HasOne(t => t.User).WithMany(u => u.Transactions).HasForeignKey(t => t.UserId);
    }
}
