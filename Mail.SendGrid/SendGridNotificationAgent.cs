using Notification.Core;
using Notification.Concerns;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Notification.Core.Providers;

namespace Notification.SendGrid
{
    public class SendGridNotificationAgent<T> : NotificationAgent<T> where T : Notification.Concerns.Notification
    {
        private string ApiKey { get; set; }

        private SendGridClient EmailClient { get; set; }

        public SendGridNotificationAgent(NotificationConfiguration config)
        {
            this.ApiKey = config.APIKey;
            this.EmailClient = new SendGridClient(this.ApiKey);
        }

        public override Task<IEnumerable<NotificationResponse>> Send(IEnumerable<T> notifications)
        {
            throw new NotImplementedException();
        }

        public async override Task<NotificationResponse> SendAsync(T notification)
        {
            try
            {
                var mail = notification as Email;
                var tos = new List<EmailAddress>();
                mail.To.ForEach(_ => tos.Add(new EmailAddress(_)));
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(new EmailAddress(mail.FromEmail), tos, notification.Subject, string.Empty, notification.Content);
                if (notification.SendNow)
                {
                    msg.SendAt = new DateTimeOffset(notification.SendOn).ToUnixTimeSeconds();
                }
                var resp = await this.EmailClient.SendEmailAsync(msg);
                return new NotificationResponse
                {
                    DeliveryStatus = DeliveryStatus.Delivered,
                    NotificationStatus = NotificationStatus.Sent
                };
            }
            catch (Exception ex)
            {

            }

            return new NotificationResponse
            {
                DeliveryStatus = DeliveryStatus.Bounced,
                NotificationStatus = NotificationStatus.Failed
            };
        }
    }    
}
 