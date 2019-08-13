using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Concerns
{
    public class Notification : INotification
    {
        public int Id { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTime SendOn { get; set; }

        public bool SendNow { get; set; }

        public NotificationType Type { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
