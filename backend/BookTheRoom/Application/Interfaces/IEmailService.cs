namespace Application.Interfaces
{
    /// <summary>
    /// Provides functionality to send emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email with the specified recipient, subject, and body.
        /// </summary>
        /// <param name="to">Recipient's email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Email message content.</param>
        void SendEmail(string to, string subject, string body);
    }
}
