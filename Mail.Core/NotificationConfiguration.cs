using Notification.Concerns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Core
{
    public class NotificationConfiguration : INotificationConfiguration
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string APIKey { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Domain {get;set; }
    }
}
