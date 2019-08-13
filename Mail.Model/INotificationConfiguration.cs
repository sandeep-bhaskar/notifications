using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Concerns
{
    public interface INotificationConfiguration
    {
        string UserName { get; set; }

        string Password { get; set; }

        string APIKey { get; set; }

        string Host { get; set; }

        int Port { get; set; }

        string Domain { get; set; }
    }
}
