using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PartyInvitationApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        // Constructor to initialize email configuration settings
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // Method to send an email asynchronously
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Retrieve SMTP server details from configuration
                var smtpServer = _config["EmailSettings:SmtpServer"];

                var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);

                var fromEmail = _config["EmailSettings:FromEmail"];

                var fromPassword = _config["EmailSettings:FromPassword"];

                // Logging email details

                Console.WriteLine($" Sending email to {toEmail}");

                Console.WriteLine($" SMTP: {smtpServer}:{smtpPort}");

                Console.WriteLine($" From: {fromEmail}");

                // Configuring  SMTP client with server details

                using var client = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

                // Create email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                // Send the email
                await client.SendMailAsync(mailMessage);

                Console.WriteLine(" Email sent successfully!");

                return true;
            }
            catch (Exception ex)
            {
                // Logging theee email sending failure details

                Console.WriteLine($" Email sending failed: {ex.Message}");

                if (ex.InnerException != null)

                {
                    Console.WriteLine($" Inner Exception: {ex.InnerException.Message}");
                }

                return false;
            }
        }
    }
}