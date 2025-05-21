using T.Domain.Models;

namespace T.Domain.Interfaces;

public interface IEmailService {
    Task SendEmailAsync(EmailMessage message);
}
