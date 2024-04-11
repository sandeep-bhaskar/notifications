using Notification.Concerns;
using Notification.Core.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Core.Contracts
{
    public interface INotificationPublisher<T> where T : NotificationAgent<Concerns.Notification>
    {
        Task<NotificationStatus> Publish<N>(N notification) where N : Concerns.Notification;

        void Publish<N>(IEnumerable<N> notifications) where N : Concerns.Notification;
    }
}
