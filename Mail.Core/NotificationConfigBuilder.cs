using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Notification.Core
{
    public class NotificationConfigBuilder
    {
        private static NotificationConfiguration NotificationConfig;

        public static NotificationConfiguration Build()
        {
            if (NotificationConfig == null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NotificationConfiguration));

                var apiConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.NotificationConfigurationFileName);

                using (FileStream fs = new FileStream(apiConfigFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader reader = new XmlTextReader(fs))
                    {
                        NotificationConfig = (NotificationConfiguration)serializer.Deserialize(reader);
                    }
                }
            }

            return NotificationConfig;
        }
    }
}
