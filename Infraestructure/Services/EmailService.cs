using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using K_All_Sonys_Notification_Api.DTO;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly NotificationMetadata _notificationMetadata;
        private readonly SmtpClient _smtpClient;
        private readonly INotIficationService _notIficationService;
        

        public EmailService(INotIficationService notificationService, NotificationMetadata notificationMetadata, SmtpClient smtpClient)
        {
            _notificationMetadata = notificationMetadata;
            _smtpClient = smtpClient;
            _notIficationService = notificationService;
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

        public async Task<NotificationResponse> SendEmail(EmailMessage message)
        {
            var notificationRespose = new NotificationResponse();
            try
            {
                
                var mimeMessage = await CreateMimeMessageFromEmailMessage(message);


                _smtpClient.Connect(_notificationMetadata.SmtpServer,
                _notificationMetadata.Port, false);
                _smtpClient.Authenticate(_notificationMetadata.UserName,
                _notificationMetadata.Password);
                _smtpClient.Send(mimeMessage);
                _smtpClient.Disconnect(true);
                notificationRespose.status = "0";



                
            }
            catch (Exception)
            {
                notificationRespose.status = "1";
            }

            var newNotification = new ApplicationCore.DTO.Notification
            {
                Message = message.Content,
                status = notificationRespose.status,
                Type = "E"
            };

            await _notIficationService.CreateNotificationAsync(newNotification);

            return notificationRespose;

        }
    }
}
