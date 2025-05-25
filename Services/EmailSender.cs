using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NawatechAuthApp.Services
{
    public class AuthMessageSenderOptions
    {
        public string? SendGridKey { get; set; }
    }

    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSender> logger, IConfiguration configuration)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
            _configuration = configuration;
        }

        public AuthMessageSenderOptions Options { get; }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                
                using var client = new SmtpClient(emailSettings["Host"], int.Parse(emailSettings["Port"]!))
                {
                    Credentials = new NetworkCredential(emailSettings["UserName"], emailSettings["Password"]),
                    EnableSsl = bool.Parse(emailSettings["EnableSsl"]!)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings["SenderEmail"]!, emailSettings["SenderName"]),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                
                mailMessage.To.Add(email);
                
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {email}");
                // For development, don't throw exception
            }
        }
    }
}