namespace DR.Domain.Common {

    public abstract class BaseEntity {
        public string Id { get; set; } = null!;
        public string MerchantId { get; set; } = null!;
    }
}