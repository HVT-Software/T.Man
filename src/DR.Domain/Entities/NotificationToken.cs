using DR.Domain.Enums;

namespace DR.Domain.Entities {

    public class NotificationToken {
        public Guid Id { get; set; }
        public string MerchantId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public EEnviroment Enviroment { get; set; }
    }
}