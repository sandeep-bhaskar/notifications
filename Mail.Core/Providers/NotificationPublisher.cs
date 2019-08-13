using Notification.Concerns;
using Notification.Contracts;
using Notification.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            catch (Exception e)
            {
            }

            return NotificationStatus.Failed;
        }

        public async void Publish<N>(IEnumerable<N> notifications) where N : Concerns.Notification
        {
            try
            {
                SendNotifications(notifications.ToList());
            }
            catch (Exception e)
            {
            }
        }

        private void SendNotifications<N>(List<N> notifications) where N : Concerns.Notification
        {
            if (notifications.Any())
            {
                try
                {
                    Parallel.ForEach(notifications, (_) => Publish(_));
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
