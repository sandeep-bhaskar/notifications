using Notification.Core;
using Notification.Concerns;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Notification.Core.Providers;
using System.Linq;

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

        public async override Task<IEnumerable<NotificationResponse>> Send(IEnumerable<T> notifications)
        {
            List<NotificationResponse> resp = new List<NotificationResponse>();
            try
            {
                notifications.ToList().ForEach(n => {
                    resp.Add(this.Send(n));
                });

                return resp;
            }
            catch (Exception ex)
            {

            }

            resp.Add(new NotificationResponse
            {
                DeliveryStatus = DeliveryStatus.Bounced,
                NotificationStatus = NotificationStatus.Failed
            });

            return resp;
        }

        public async override Task<NotificationResponse> SendAsync(T notification)
        {
            try
            {
                var msg = this.GetEmailMessage(notification as Email);
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

        public override NotificationResponse Send(T notification)
        {
            try
            {
                var msg = this.GetEmailMessage(notification as Email);
                this.EmailClient.SendEmailAsync(msg);
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

        private SendGridMessage GetEmailMessage(Email email)
        {
            var tos = new List<EmailAddress>();
            email.To.ForEach(_ => tos.Add(new EmailAddress(_)));
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(new EmailAddress(email.FromEmail), tos, email.Subject, string.Empty, email.Content);
            if (email.SendNow)
            {
                msg.SendAt = new DateTimeOffset(email.SendOn).ToUnixTimeSeconds();
            }

            if (email.Attachments != null)
            {
                email.Attachments.ForEach(at =>
                {
                    msg.Attachments.Add(new Attachment { Content = System.Text.Encoding.UTF8.GetString(at.Content), Filename = at.Name });
                });
            }

            if (email.CC != null)
            {
                email.CC.ForEach(cc =>
                {
                    msg.AddCc(cc);
                });
            }

            if (email.BCC != null)
            {
                email.BCC.ForEach(Bcc =>
                {
                    msg.AddBcc(Bcc);
                });
            }

            return msg;
        }
    }
}
 