using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using K_All_Sonys_Notification_Api.DTO;
using MailKit.Net.Smtp;
using MimeKit;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
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
        private static readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(5),
                (exception, timeSpan) => {
                    Console.WriteLine("OnBreaK:  Circuito roto");
                },
                () =>
                {
                    Console.WriteLine("OnReset: Cricuito reestablecido");
                });

        private static readonly AsyncRetryPolicy _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(2, retryAttempt => {
                    var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    Console.WriteLine($"Waiting {timeToWait.TotalSeconds} seconds");
                    return timeToWait;
                }
                );



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

            var notificationRespose = new NotificationResponse
            {
                statusCode = "1",
                message = "Email sent Error"
            };

            try
            {
                Console.WriteLine($"Estado del circuito: {_circuitBreakerPolicy.CircuitState}");
                await _circuitBreakerPolicy.ExecuteAsync(async () => 
                {
                    try
                    {
                        var mimeMessage = await CreateMimeMessageFromEmailMessage(message);

                        await _smtpClient.ConnectAsync(_notificationMetadata.SmtpServer,
                        _notificationMetadata.Port, false);
                        await _smtpClient.AuthenticateAsync(_notificationMetadata.UserName,
                        _notificationMetadata.Password);

                        await _smtpClient.SendAsync(mimeMessage);

                        notificationRespose.statusCode = "0";
                        notificationRespose.message = "Email sent successfully";
                    }
                    finally
                    {
                        await _smtpClient.DisconnectAsync(true);
                    }
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro al enviar correo: {ex.Message}");
            }
            finally
            {
                var newNotification = new ApplicationCore.DTO.Notification
                {
                    Message = message.Content,
                    status = notificationRespose.statusCode,
                    Type = "E",
                    Reciever = message.Reciever,
                    DateNotification = DateTime.UtcNow.ToLocalTime()
                };

                await persistNotification(newNotification);
            }

            return notificationRespose;
        }

        public async Task<Notification> persistNotification(ApplicationCore.DTO.Notification newNotification) {
            try
            {
                newNotification = await _retryPolicy.ExecuteAsync(async () => await _notIficationService.CreateNotificationAsync(newNotification));
                
            }
            catch (Exception ex) {

                Console.WriteLine(ex.Message);
            }
            return newNotification;
        }
    }
}