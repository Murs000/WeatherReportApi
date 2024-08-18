using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Settings;

namespace WeatherReport.Business.Services.Implementations;


public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        var settings = emailSettings.Value;
        _fromEmail = settings.FromEmail;

        _smtpClient = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
        {
            Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
        {
            IsBodyHtml = true
        };
        await _smtpClient.SendMailAsync(mailMessage);
    }
}