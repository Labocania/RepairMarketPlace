using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace RepairMarketPlace.Infrastructure.Services
{
    public class NetworkClient
    {
        public EmailServerSettings Settings { get; private set; }
        private readonly ILogger<NetworkClient> _logger;

        public NetworkClient(EmailServerSettings settings, ILogger<NetworkClient> logger)
        {
            Settings = settings;
            _logger = logger;
        }

        public async Task SendAsync(MimeMessage message)
        {
            using (SmtpClient client = new())
            {
                try
                {
                    client.Connect(Settings.SMTPServer, Settings.SMTPPortNumber, SecureSocketOptions.StartTls);
                }
                catch (SmtpCommandException ex)
                {
                    _logger.LogError("Error trying to connect: {0}", ex.Message);
                    _logger.LogError("\tStatusCode: {0}", ex.StatusCode);
                }
                catch(SmtpProtocolException ex)
                {
                    _logger.LogError("Protocol error while trying to connect: {0}", ex.Message);
                }

                try
                {
                    client.Authenticate(Settings.SMTPLogin, Settings.SMTPPassword);
                }
                catch (AuthenticationException ex)
                {
                    _logger.LogError("Invalid user name or password. Link: {0}", ex.Message);
                }
                catch (SmtpCommandException ex)
                {
                    _logger.LogError("Error trying to authenticate: {0}", ex.Message);
                    _logger.LogError("\tStatusCode: {0}", ex.StatusCode);
                }
                catch (SmtpProtocolException ex)
                {
                    _logger.LogError("Protocol error while trying to authenticate: {0}", ex.Message);
                }

                try
                {
                    await client.SendAsync(message);
                }
                catch (SmtpCommandException ex)
                {
                    _logger.LogError("Error sending message: {0}", ex.Message);
                    _logger.LogError("\tStatusCode: {0}", ex.StatusCode);

                    switch (ex.ErrorCode)
                    {
                        case SmtpErrorCode.RecipientNotAccepted:
                            _logger.LogError("\tRecipient not accepted: {0}", ex.Mailbox);
                            break;
                        case SmtpErrorCode.SenderNotAccepted:
                            _logger.LogError("\tSender not accepted: {0}", ex.Mailbox);
                            break;
                        case SmtpErrorCode.MessageNotAccepted:
                            _logger.LogError("\tMessage not accepted.");
                            break;
                        default:
                            _logger.LogError(ex.Message);
                            break;
                    }
                }
                catch (SmtpProtocolException ex)
                {
                    _logger.LogError("Protocol error while sending message: {0}", ex.Message);
                }
                catch(ServiceNotAuthenticatedException ex)
                {
                    _logger.LogError("Authentication Required. Learn more at: {0}", ex.Message);
                }

                await client.DisconnectAsync(true);
            }
        }
    }
}
