using K_All_Sonys_Notification_Api.Entities;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K_All_Sonys_Notification_Api.Interfaces
{
    public interface IEmailService
    {
        public string SendEmail(EmailMessage message);
        public MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message);
    }
}
