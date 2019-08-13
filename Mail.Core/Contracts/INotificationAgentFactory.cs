using Notification.Concerns;
using Notification.Core.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Contracts
{
    public interface INotificationAgentFactory<T> where T : Concerns.Notification
    {
        Task<NotificationResponse> SendNotification<N>(N notification) where N : Concerns.Notification;

        Task<IEnumerable<NotificationResponse>> SendNotifications<N>(IEnumerable<N> notifications) where N : Concerns.Notification;
    }
}
