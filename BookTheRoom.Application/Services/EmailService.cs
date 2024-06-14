using BookTheRoom.Application.Settings;
using BookTheRoom.Application.Interfaces;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MimeKit;
using Microsoft.Extensions.Options;


namespace BookTheRoom.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _options;

        public EmailService(IOptions<EmailSettings> options)
        {
            _options = options.Value;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.EmailUsername));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_options.EmailHost, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.EmailUsername, _options.EmailPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }      
    }
}
