using ApplicationCore.DTO;
using K_All_Sonys_Notification_Api.DTO;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IEmailService
    {
        public Task<NotificationResponse> SendEmail(EmailMessage message);
        public Task<MimeMessage> CreateMimeMessageFromEmailMessage(EmailMessage message);
    }
}
