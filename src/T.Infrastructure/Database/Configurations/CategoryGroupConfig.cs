using T.Domain.Extensions;

namespace T.Infrastructure.Database.Configurations;

public class CategoryGroupConfig : IEntityTypeConfiguration<Category> {
    public void Configure(EntityTypeBuilder<Category> builder) {
        builder.ToTable(nameof(Category));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.Budget).HasCurrencyPrecision().IsRequired();
        builder.Property(o => o.CreateAt).HasDateConversion().IsRequired();

        // relationship
        builder.HasOne(o => o.Merchant).WithMany(o => o.Categories).HasForeignKey(o => o.MerchantId);
    }
}
