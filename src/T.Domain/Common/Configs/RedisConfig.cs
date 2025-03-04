namespace T.Domain.Common.Configs {

    public class RedisConfig {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
