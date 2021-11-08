using RepairMarketPlace.ApplicationCore.Interfaces;
using System.Threading.Tasks;
using MimeKit;

namespace RepairMarketPlace.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly NetworkClient _client;
        public EmailSender(NetworkClient client)
        {
            _client = client;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Jackson Bright", _client.Settings.SMTPLogin));
            mailMessage.To.Add(new MailboxAddress(email, email));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("html")
            {
                Text = message
            };

            await _client.SendAsync(mailMessage);
        }
    }
}
