using Notification.Concerns;
using Notification.Contracts;
using Notification.Core.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Core.Contracts
{
    public interface INotificationPublisher<T> where T : NotificationAgent<Concerns.Notification>
    {
        Task<NotificationStatus> Publish<N>(N notification) where N : Concerns.Notification;

        void Publish<N>(IEnumerable<N> notifications) where N : Concerns.Notification;
    }
}
