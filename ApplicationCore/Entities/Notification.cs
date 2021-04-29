using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Notification
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
