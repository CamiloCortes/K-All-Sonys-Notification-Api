using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using K_All_Sonys_Notification_Api.DTO;
using MailKit.Net.Smtp;
using MimeKit;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly NotificationMetadata _notificationMetadata;
        private readonly SmtpClient _smtpClient;
        private readonly INotIficationService _notIficationService;

        private static readonly AsyncCircuitBreakerPolicy<bool>
                         basicCircuitBreakerPolicy = Policy.HandleResult<bool>(r => r == false)
                             .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 2, durationOfBreak: TimeSpan.FromSeconds(20));



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

                try {
                    await _smtpClient.ConnectAsync(_notificationMetadata.SmtpServer,
                    _notificationMetadata.Port, false);
                    await _smtpClient.AuthenticateAsync(_notificationMetadata.UserName,
                    _notificationMetadata.Password);

                }
                catch (Exception) { 

                }
                await basicCircuitBreakerPolicy.ExecuteAsync(async () =>
                 {
                     return await Task.FromResult(_smtpClient.IsConnected);
                 });

                if (basicCircuitBreakerPolicy.CircuitState == 0)
                {
                    await _smtpClient.SendAsync(mimeMessage);
                    await _smtpClient.DisconnectAsync(true);

                    notificationRespose.statusCode = "0";
                    notificationRespose.message = "Email sent successfully";
                }
                else
                {

                    notificationRespose.statusCode = "1";
                    notificationRespose.message = "Email sent Error";
                }

            }
            catch (Exception)
            {

                notificationRespose.statusCode = "1";
                notificationRespose.message = "Email sent Error";
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true);
            }

            var newNotification = new ApplicationCore.DTO.Notification
            {
                Message = message.Content,
                status = notificationRespose.statusCode,
                Type = "E",
                Reciever = message.Reciever,
                DateNotification = DateTime.UtcNow.ToLocalTime()
            };

             await persistNotification(newNotification);

            return notificationRespose;

        }

        public async Task<Notification> persistNotification(ApplicationCore.DTO.Notification newNotification) {
            try
            {
                newNotification = await _notIficationService.CreateNotificationAsync(newNotification);
            }
            catch (Exception ex) {

                Console.WriteLine(ex.Message);
            }
            return newNotification;
        }
    }
}
