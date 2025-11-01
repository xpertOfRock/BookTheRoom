namespace Application.Interfaces
{
    public interface IOtpService
    {
        Task SendCode(string to);
        Task<bool> VerifyCode(string email, string code);
    }
}
