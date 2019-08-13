using Notification.Concerns;
using Notification.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Core.Providers
{
    public abstract class NotificationAgent<T> : INotificationAgent<T> where T : Concerns.Notification
    {
        public abstract NotificationResponse Send(T notification);

        public abstract Task<IEnumerable<NotificationResponse>> Send(IEnumerable<T> notifications);

        public abstract Task<NotificationResponse> SendAsync(T notification);
    }
}
