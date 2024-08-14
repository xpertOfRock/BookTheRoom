using Application.Interfaces;
using Application.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Application.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _options;

        public EmailService(IOptions<EmailSettings> options)
        {
            _options = options.Value;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.EmailUsername));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = body };

            using var smtp = new SmtpClient();
            var connectionResult = smtp.ConnectAsync(_options.EmailHost, _options.EmailPort, SecureSocketOptions.StartTls);
            var authResult = smtp.AuthenticateAsync(_options.EmailUsername, _options.EmailPassword);
            var sendMessageResult = smtp.SendAsync(email);
            var dissconectionResult = smtp.DisconnectAsync(true);
        }
    }
}
