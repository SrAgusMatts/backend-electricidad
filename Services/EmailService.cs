using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Backend.Interfaces;

namespace Backend.Services
{ 

    // 2. La Implementación
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Electricidad Mattos", _config["AppSettings:EmailFrom"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlBody;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_config["AppSettings:SmtpHost"], int.Parse(_config["AppSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config["AppSettings:SmtpUser"], _config["AppSettings:SmtpPass"]);
                await smtp.SendAsync(email);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}