#region

using T.Domain.Extensions;

#endregion

namespace T.Infrastructure.Database.Configurations;

public class CategoryConfig : IEntityTypeConfiguration<Category> {
    public void Configure(EntityTypeBuilder<Category> builder) {
        builder.ToTable(nameof(Category));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.Budget).HasCurrencyPrecision().IsRequired();
        builder.Property(o => o.CreateAt).HasDateConversion().IsRequired();
        builder.Property(o => o.Description).HasMaxLength(2000).IsRequired();

        // relationship
        builder.HasOne(o => o.Merchant).WithMany(o => o.Categories).HasForeignKey(o => o.MerchantId);
        builder.HasMany(o=> o.Transactions).WithOne(o => o.Category).HasForeignKey(o => o.CategoryId);
    }
}
