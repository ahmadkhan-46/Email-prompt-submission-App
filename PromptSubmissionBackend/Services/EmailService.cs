using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PromptSubmissionBackend.Services;

public class EmailService(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var fromEmail = _config["EmailSettings:From"];
        var smtpHost = _config["EmailSettings:SmtpHost"];
        var smtpPortStr = _config["EmailSettings:SmtpPort"];
        var smtpUser = _config["EmailSettings:SmtpUser"];
        var smtpPass = _config["EmailSettings:SmtpPass"];

        
        if (!int.TryParse(smtpPortStr, out var smtpPort))
            throw new ArgumentException("SMTP port is invalid or not configured.");

        
        if (string.IsNullOrWhiteSpace(fromEmail)) throw new ArgumentNullException(nameof(fromEmail));
        if (string.IsNullOrWhiteSpace(toEmail)) throw new ArgumentNullException(nameof(toEmail));

        var mail = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        mail.To.Add(toEmail);

        using var smtp = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true    
        };

        await smtp.SendMailAsync(mail);
    }
}
