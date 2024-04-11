using Notification.Concerns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.Contracts
{
    public interface INotificationAgent<T> where T : Concerns.Notification
    {
        Task<NotificationResponse> SendAsync(T notification);

        Task<IEnumerable<NotificationResponse>> Send(IEnumerable<T> notifications);

        NotificationResponse Send(T notifications);
    }
}
