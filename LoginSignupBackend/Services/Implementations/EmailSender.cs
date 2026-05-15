using System.Net;
using System.Net.Mail;
using LoginSignup.Services.Interfaces;
using LoginSignupBackend.Services.Interfaces;

namespace LoginSignup.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var smtpHost = _config["Email:Host"];
            var smtpPort = int.Parse(_config["Email:Port"]);
            var fromEmail = _config["Email:From"];

            var smtpUser = _config["Email:Username"];
            var smtpPass = _config["Email:Password"];

            var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
                UseDefaultCredentials = false
            };


            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail, "LoginSignup App"),
                Subject = "Password Reset OTP",
                Body = $"Your OTP for password reset is: {otp}",
                IsBodyHtml = false
            };

            mail.To.Add(toEmail);

            await client.SendMailAsync(mail);
        }
    }
}
