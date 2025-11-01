namespace Infrastructure.Services
{
    public class OtpService
    (
        IEmailService emailService,
        IDistributedCache distributedCache
    ) : IOtpService
    {
        public async Task SendCode(string to)
        {
            string code = GenerateCode();

            await distributedCache.SetStringAsync(to, code);

            const string Subject = "Password reset";

            string body = $"Your one time password: {code}. If you didn't perform reseting the password, please ignore this message.";

            emailService.SendEmail(to, Subject, body);
        }

        public Task<bool> VerifyCode(string email, string code)
        {
            throw new NotImplementedException();
        }
        private string GenerateCode()
        {
            Random random = new();

            var generated = random.Next(100_000, 999_999);

            return generated.ToString();
        }
    }
}
