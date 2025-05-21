using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using T.Domain.Common.Configs;
using T.Domain.Interfaces;
using T.Domain.Models;

namespace T.Domain.Services;

public class EmailService(IOptions<EmailConfig> setting) : IEmailService {
    private readonly EmailConfig config = setting.Value;

    public async Task SendEmailAsync(EmailMessage message) {
        var mime = new MimeMessage();
        mime.From.Add(MailboxAddress.Parse(config.From));
        mime.To.Add(MailboxAddress.Parse(message.To));
        mime.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder {
            HtmlBody = message.IsHtml ? message.Body : null,
            TextBody = message.IsHtml ? null : message.Body,
        };
        mime.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(config.SmtpHost, config.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(config.Username, config.Password);
        await client.SendAsync(mime);
        await client.DisconnectAsync(true);
    }
}
