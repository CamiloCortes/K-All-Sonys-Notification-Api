using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTO
{
    public class Notification
    {
            public long Id { get; set; }
            public string Type { get; set; }
            public string Message { get; set; }
            public string status { get; set; }
            public string Reciever { get; set; }
            public DateTime? DateNotification { get; set; }
    }
}
