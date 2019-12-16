using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string Receiver { get; set; }

        public Notification(string content, string receiver)
        {
            Content = content;
            Timestamp = DateTime.Now;
            Receiver = receiver;
        }
    }
}
