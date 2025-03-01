//// Services/EmailService.cs
//using System.Net;
//using System.Net.Mail;
//using System.Threading.Tasks;

//namespace PartyInvitationApp.Services
//{
//    public class EmailService
//    {
//        private readonly string smtpServer = "smtp.gmail.com";
//        private readonly int smtpPort = 587;
//        private readonly string fromEmail = "jiyapandit01@gmail.com";
//        private readonly string fromPassword = "bfbb jssl divn lkgc";

//        public async Task SendEmailAsync(string toEmail, string subject, string body)
//        {
//            using var client = new SmtpClient(smtpServer)
//            {
//                Port = smtpPort,
//                Credentials = new NetworkCredential(fromEmail, fromPassword),
//                EnableSsl = true
//            };

//            var mailMessage = new MailMessage
//            {
//                From = new MailAddress(fromEmail),
//                Subject = subject,
//                Body = body,
//                IsBodyHtml = true
//            };
//            mailMessage.To.Add(toEmail);

//            await client.SendMailAsync(mailMessage);
//        }
//    }
//}


using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
namespace PartyInvitationApp.Services
{

    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 465; // Use 465 for SSL or 587 for TLS
        private readonly string _senderEmail = "jiyapandit01@gmail.com";
        private readonly string _senderPassword = "bfbb jssl divn lkgc"; // Use App Password for Gmail

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}