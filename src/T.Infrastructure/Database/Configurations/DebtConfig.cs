#region

using T.Domain.Extensions;

#endregion

namespace T.Infrastructure.Database.Configurations;

public class DebtConfig : IEntityTypeConfiguration<Debt> {
    public void Configure(EntityTypeBuilder<Debt> builder) {
        builder.ToTable(nameof(Debt));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.BorrowerName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Amount).IsRequired().HasCurrencyPrecision();
        builder.Property(x => x.ExpectedReturnDate).IsRequired().HasDateConversion();
        builder.Property(x => x.Status).IsRequired();

        builder.Property(x => x.Date).IsRequired().HasDateConversion();
        builder.Property(x => x.CreatedAt).IsRequired().HasDateConversion();

        // Foreign key
        builder.HasOne(x => x.User).WithMany(o => o.Debit).HasForeignKey(x => x.UserId);
    }
}
