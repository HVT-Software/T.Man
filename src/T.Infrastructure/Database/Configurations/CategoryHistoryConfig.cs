#region

using T.Domain.Extensions;

#endregion

namespace T.Infrastructure.Database.Configurations;

public class CategoryHistoryConfig : IEntityTypeConfiguration<CategoryHistory> {
    public void Configure(EntityTypeBuilder<CategoryHistory> builder) {
        builder.ToTable(nameof(CategoryHistory));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Month).HasDateConversion().IsRequired();
        builder.Property(o => o.BudgetAmount).HasCurrencyPrecision().IsRequired();
        builder.Property(o => o.UsedAmount).HasCurrencyPrecision().IsRequired();
        builder.Property(o => o.CreatedAt).HasDateConversion().IsRequired();

        // relationship
        builder.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.CategoryId);
    }
}
