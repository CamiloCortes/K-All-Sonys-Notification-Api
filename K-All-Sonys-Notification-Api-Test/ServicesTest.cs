using K_All_Sonys_Notification_Api.DTO;
using K_All_Sonys_Notification_Api.Interfaces;
using K_All_Sonys_Notification_Api.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace K_All_Sonys_Notification_Api_Test
{
    public class ServicesTest
    {
        private NotificationMetadata _notificationMetadata;
        private SmtpClient _smtpClient;
        private EmailService _emailService;
        
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _notificationMetadata = new NotificationMetadata();
            _smtpClient = new SmtpClient();

            _notificationMetadata.SmtpServer = config["NotificationMetadata:SmtpServer"];
            _notificationMetadata.Port = Int16.Parse(config["NotificationMetadata:Port"]);
            _notificationMetadata.UserName = config["NotificationMetadata:Username"];
            _notificationMetadata.Password = config["NotificationMetadata:Password"];

            _emailService = new EmailService(_notificationMetadata, _smtpClient);

        }

        [Test]
        public async Task SendEmailTest()
        {

            var emailMessage = new EmailMessage();

            emailMessage.Reciever = "camiloandrescortesfuquene@gmail.com";
            emailMessage.Sender = "kallsonysnotifications@gmail.com";
            emailMessage.Subject = "UNIT TEST";
            emailMessage.Content = "UNIT TEST";

            Assert.AreEqual(await _emailService.SendEmail(emailMessage), "Email sent successfully");
        }
    }
}