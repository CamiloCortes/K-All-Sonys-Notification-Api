using K_All_Sonys_Notification_Api.DTO;
using K_All_Sonys_Notification_Api.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K_All_Sonys_Notification_Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly NotificationMetadata _notificationMetadata;
        private readonly SmtpClient _smtpClient;

        public EmailService(NotificationMetadata notificationMetadata, SmtpClient smtpClient)
        {
            _notificationMetadata = notificationMetadata;
            _smtpClient = smtpClient;
        }
        public async Task<MimeMessage> CreateMimeMessageFromEmailMessage(EmailMessage message)
        {

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("K_All_Sonys", message.Sender));
            mimeMessage.To.Add(new MailboxAddress("K_All_Sonys", message.Reciever));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            { Text = message.Content };
            return await Task.FromResult(mimeMessage);
        }

        public async Task<string> SendEmail(EmailMessage message)
        {
            try
            {
                var mimeMessage = await CreateMimeMessageFromEmailMessage(message);


                _smtpClient.Connect(_notificationMetadata.SmtpServer,
                _notificationMetadata.Port, false);
                _smtpClient.Authenticate(_notificationMetadata.UserName,
                _notificationMetadata.Password);
                _smtpClient.Send(mimeMessage);
                _smtpClient.Disconnect(true);

                return await Task.FromResult("Email sent successfully");
            }
            catch (Exception)
            {

                return "Email sent Error";
            }


        }
    }
}
