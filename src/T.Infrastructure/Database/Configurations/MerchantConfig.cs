#region

using T.Domain.Extensions;

#endregion

namespace T.Infrastructure.Database.Configurations;

internal class MerchantConfig : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable(nameof(Merchant));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Code).HasMaxLength(50).IsRequired();
        builder.Property(o => o.Name).HasMaxLength(255).IsRequired();
        builder.Property(o => o.SearchName).HasMaxLength(255).IsRequired();

        // index
        builder.Property(o => o.CreatedAt).HasDateConversion().IsRequired();

        // relationship
        builder.HasMany(o => o.Users).WithOne(o => o.Merchant).HasForeignKey(o => o.MerchantId);
        builder.HasMany(o => o.Categories).WithOne(o => o.Merchant).HasForeignKey(o => o.MerchantId);
    }
}
