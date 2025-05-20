#region

using T.Domain.Extensions;

#endregion

namespace T.Infrastructure.Database.Configurations;

public class TransactionConfig : IEntityTypeConfiguration<Transaction> {
    public void Configure(EntityTypeBuilder<Transaction> builder) {
        builder.ToTable(nameof(Transaction));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Amount).HasCurrencyPrecision().IsRequired();
        builder.Property(o => o.Date).HasDateConversion().IsRequired();
        builder.Property(o => o.CreatedAt).HasDateConversion().IsRequired();
        builder.Property(o => o.Description).HasMaxLength(500);
        builder.Property(o => o.Type).IsRequired();

        builder.HasOne(o => o.User).WithMany(u => u.Transactions).HasForeignKey(o => o.UserId);
        builder.HasOne(o => o.Category).WithMany(c => c.Transactions).HasForeignKey(o => o.CategoryId);
    }
}
