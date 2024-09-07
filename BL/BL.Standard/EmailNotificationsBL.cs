using BL.Interfaces;
using Common.Configurations;
using Entities;
using MimeKit;
using System.Web;
using MailKit.Net.Smtp;
using Tools.SymmetricEncryption.AesEncryption;
using Microsoft.Extensions.Options;

namespace BL.Standard
{
    public class EmailNotificationsBL : IEmailNotificationsBL
    {
        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly AesEncryption _encryption;

        public EmailNotificationsBL(IOptions<SmtpConfiguration> smtpConfiguration, AesEncryption encryption)
        {
            _smtpConfiguration = smtpConfiguration.Value;
            _encryption = encryption;
        }

        public async Task SendRecoveryPasswordEmailAsync(string toEmail, User user)
        {
            var token = HttpUtility.UrlEncode(_encryption.Encrypt(user));
            var url = $"https://localhost:7075/Account/VerifyUser?tokenKey={token}";

            string subject = "Recovery password";
            string body = $"To log in without a password, follow the link provided: <a href=\"{url}\" target=\"_blank\">Recover password</a>";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Whac a Mole Support", _smtpConfiguration.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.UseSsl);
                await client.AuthenticateAsync(_smtpConfiguration.SenderEmail, _smtpConfiguration.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
