namespace T.Domain.Common.Configs;

public class EmailConfig {
    public string SmtpHost { get; set; } = string.Empty; // Ví dụ: "smtp.gmail.com"
    public int    SmtpPort { get; set; }                 // Ví dụ: 587
    public string Username { get; set; } = string.Empty; // Tài khoản SMTP
    public string Password { get; set; } = string.Empty; // Mật khẩu hoặc app-specific password
    public string From     { get; set; } = string.Empty; // Địa chỉ email gửi đi
}
