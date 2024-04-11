using Notification.Concerns;
using Notification.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Core.Providers
{
    public class NotificationPublisher<T> : INotificationPublisher<T> where T : NotificationAgent<Concerns.Notification>
    {
        public INotificationConfiguration NotificationConfiguration { get; set; }

        public NotificationPublisher(INotificationConfiguration notificationConfiguration)
        {
            this.NotificationConfiguration = notificationConfiguration;
            NotificationAgentFactory<T>.CreateAgentInstance(notificationConfiguration);
        }


        public async Task<NotificationStatus> Publish<N>(N notification) where N : Concerns.Notification
        {
            try
            {
                var response = await NotificationAgentFactory<T>.SendNotification<N>(notification);
                notification.Status = response.NotificationStatus;
                return notification.Status;
            }
            catch (Exception)
            {
                return NotificationStatus.Failed;
            }
        }

        public void Publish<N>(IEnumerable<N> notifications) where N : Concerns.Notification
        {
            SendNotifications(notifications.ToList());
        }

        private void SendNotifications<N>(List<N> notifications) where N : Concerns.Notification
        {
            if (notifications.Any())
            {
                try
                {
                    Parallel.ForEach(notifications, (_) => Publish(_));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
