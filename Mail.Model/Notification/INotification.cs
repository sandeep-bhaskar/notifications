using System;

namespace Notification.Concerns
{
    public interface INotification
    {
      int Id { get; set; }

      NotificationStatus Status { get; set; }

      DateTime SendOn { get; set; }

      bool SendNow { get; set; }

      NotificationType Type { get; set; }

      string Subject { get; set; }

      string Content { get; set; }
    }
}
