using System;
using System.Collections.Generic;
using System.Text;

namespace KAllSonysNotificationApi.ApplicationCore.Entities
{
    public class Notification
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Reciever { get; set; }
        public DateTime? DateNotification { get; set; }
    }
}
