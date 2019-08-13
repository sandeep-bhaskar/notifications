using Notification.Core;
using Notification.Concerns;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Notification.Core.Providers;

namespace Notification.ExchangeMailService
{
    public class ExchangeServerNotification : NotificationAgent<Email>
    {
        public ExchangeService ExchangeService = null;
        public ExchangeServerNotification(string exchangeServerUserName,
            string exchangeServerPassword,
            string exchangeServerDomain,
            string DefaultFromEmail)
        {
            ExchangeService = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            ExchangeService.Credentials = new WebCredentials(exchangeServerUserName, exchangeServerPassword, exchangeServerDomain);
            ExchangeService.AutodiscoverUrl(DefaultFromEmail);            
        }

        public override Task<IEnumerable<NotificationResponse>> Send(IEnumerable<Email> notifications)
        {
            throw new NotImplementedException();
        }

        public override async Task<NotificationResponse> SendAsync(Email notification)
        {
            try
            {
                EmailMessage emailMessage = new EmailMessage(this.ExchangeService);
                emailMessage.Subject = notification.Subject;
                emailMessage.Body = new MessageBody(notification.Content);
                if (notification.Priority == MailPriority.High)
                {
                    emailMessage.Importance = Importance.High;
                }

                notification.To.ForEach(to =>
                {
                    emailMessage.ToRecipients.Add(to);
                });

                if (notification.CC != null)
                {
                    notification.CC.ForEach(to =>
                    {
                        emailMessage.CcRecipients.Add(to);
                    });
                }

                if (notification.BCC != null)
                {
                    notification.BCC.ForEach(to =>
                    {
                        emailMessage.BccRecipients.Add(to);
                    });
                }

                if (notification.Attachments != null)
                {
                    notification.Attachments.ForEach(at =>
                    {
                        emailMessage.Attachments.AddFileAttachment(at.Name, at.Content);
                    });
                }

                emailMessage.From = notification.FromEmail;
                emailMessage.Save();
                emailMessage.SendAndSaveCopy();

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
