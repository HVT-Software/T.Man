namespace DR.Domain.Common {

    public class RedisConfig {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}