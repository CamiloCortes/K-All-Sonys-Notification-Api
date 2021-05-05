using K_All_Sonys_Notification_Api.DTO;
using ApplicationCore.Interfaces;
using Infraestructure.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Infraestructure.Contexts;
using Microsoft.EntityFrameworkCore;
using KAllSonysNotificationApi.Infraestructure.Repositories;

namespace K_All_Sonys_Notification_Api_Test
{
    public class ServicesTest
    {
        private NotificationMetadata _notificationMetadata;
        private SmtpClient _smtpClient;
        private EmailService _emailService;
        private NotificationService _notificationService;
        private NotificationContext _dbContext;
        private NotificationsRepository _notificationsRepository;


        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        DbContextOptions<NotificationContext> dbContextOptions = new DbContextOptionsBuilder<NotificationContext>()
       .UseMySql(config["ConnectionStrings:DbNotifications"])
       .Options;
            _dbContext = new NotificationContext(dbContextOptions);
            _notificationsRepository = new NotificationsRepository(_dbContext);

            _notificationMetadata = new NotificationMetadata();
            _smtpClient = new SmtpClient();
            _notificationService = new NotificationService(_notificationsRepository);

            _notificationMetadata.SmtpServer = config["NotificationMetadata:SmtpServer"];
            _notificationMetadata.Port = Int16.Parse(config["NotificationMetadata:Port"]);
            _notificationMetadata.UserName = config["NotificationMetadata:Username"];
            _notificationMetadata.Password = config["NotificationMetadata:Password"];

            

            _emailService = new EmailService(_notificationService, _notificationMetadata, _smtpClient);

        }

        [Test]
        public async Task SendEmailTest()
        {

            var emailMessage = new EmailMessage();

            emailMessage.Reciever = "camiloandrescortesfuquene@gmail.com";
            emailMessage.Sender = "kallsonysnotifications@gmail.com";
            emailMessage.Subject = "UNIT TEST";
            emailMessage.Content = "UNIT TEST";
            var notificationResponse = await _emailService.SendEmail(emailMessage);
            Assert.AreEqual( "0", notificationResponse.statusCode);
        }
    }
}