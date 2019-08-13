using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Concerns
{
    public class NotificationResponse
    {
        public NotificationResponse()
        {
            this.NotificationStatus = NotificationStatus.Failed;
        }

        public virtual string ResponseId { get; set; }

        public Exception Exception { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
