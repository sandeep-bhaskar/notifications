using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Concerns;
using Notification.Contracts;
using Notification.Core.Providers;

namespace Notification.Core
{
    public sealed class NotificationAgentFactory<T> where T : NotificationAgent<Concerns.Notification>
    {
        private NotificationAgentFactory()
        {            
        }

        private static Object _mutex = new Object();

        private static T NotificationAgentInstance;

        public static T CreateAgentInstance(INotificationConfiguration args)
        {
            if (NotificationAgentInstance == null)
            {
                lock (_mutex) // now I can claim some form of thread safety...
                {
                    NotificationAgentInstance = (T)Activator.CreateInstance(typeof(T), args);
                }
            }
            return NotificationAgentInstance;
        }


        public static async Task<NotificationResponse> SendNotification<N>(N notification)where N : Concerns.Notification
        {
            try
            {
                return await NotificationAgentInstance.SendAsync(notification);
            }
            catch (Exception ex)
            {
                //// Failed to send email notification
                throw ex;
            }
        }

        public static async Task<IEnumerable<NotificationResponse>> SendNotifications<N>(IEnumerable<N> notifications) where N : Concerns.Notification
        {
            var responses = new List<Task<NotificationResponse>>();

            try
            {
                responses = notifications.Select(n => NotificationAgentInstance.SendAsync(n)).ToList();
            }
            catch (Exception ex)
            {
                //// Failed to send email notification
                throw ex;
            }

            return await Task.WhenAll(responses);
        }
    }
}
