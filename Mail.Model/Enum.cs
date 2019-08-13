using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Concerns
{
    public enum NotificationType
    {
        None = 0,
        SMS = 1,
        Email = 2,
        MobilePush = 3,
        BrowserPush = 4,
        Webhook = 5
    }

    public enum NotificationStatus
    {
        NotSent = 0,
        Sent = 1,
        Failed = 2
    }

    public enum DeliveryStatus
    {
        None = 0,
        Pending = 1,
        Delivered = 2,
        Bounced = 3,
        PartiallyBounced = 4
    }

    public enum MailPriority
    {
        Normal = 0,
        Low = 1,
        High = 2
    }

    public enum MailCarrierTypes
    {
        SMTP=0,
        ExhangeMailService,
        SendGrid
    }
}
