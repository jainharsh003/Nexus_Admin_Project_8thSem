namespace LoginSignupBackend.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendOtpEmailAsync(string toEmail, string otp);
    }
}
