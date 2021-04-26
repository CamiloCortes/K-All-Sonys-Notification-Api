using K_All_Sonys_Notification_Api.DTO;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K_All_Sonys_Notification_Api.Interfaces
{
    public interface IEmailService
    {
        public Task<string> SendEmail(EmailMessage message);
        public Task<MimeMessage> CreateMimeMessageFromEmailMessage(EmailMessage message);
    }
}
