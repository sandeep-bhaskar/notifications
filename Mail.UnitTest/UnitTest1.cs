using Notification.Core;
using Notification.Concerns;
using Xunit;
using Notification.Core.Providers;
using Notification.SMTP;
using Notification.SendGrid;

namespace Mail.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {

            var config = NotificationConfigBuilder.Build();
            var args = new NotificationConfiguration
            {
                Host = config.Host,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password
            };
            var smtp = new Email
            {
                Subject = "mailtosandeepone@gmail.com",
                To = new System.Collections.Generic.List<string> { "mailtosandeeptwo@gmail.com" },
                Content = "Hello world"
            };

            var publisher = new NotificationPublisher<SMTPNotificationAgent<Notification.Concerns.Notification>>(args);
            _ = publisher.Publish(smtp);

            // await obj.SendAsync(

        }

        [Fact]
        public async void SendGridTest()
        {
            var config = NotificationConfigBuilder.Build();
            var sgc = new NotificationConfiguration { APIKey = config.APIKey };
            var sendgrid = new Email
            {
                Subject = "mailtosandeepone@gmail.com",
                To = new System.Collections.Generic.List<string> { "mailtosandeeptwo@gmail.com" },
                Content = "Hello world"
            };
            var publisher = new NotificationPublisher<SendGridNotificationAgent<Notification.Concerns.Notification>>(sgc);
            _ = publisher.Publish(sendgrid);

        }
    }
}
