using Notification.Core;
using Notification.Concerns;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Notification.Core.Providers;
using System.Linq;

namespace Notification.ExchangeMailService
{
    public class ExchangeServerNotificationAgent : NotificationAgent<Email>
    {
        public ExchangeService ExchangeService = null;
        public ExchangeServerNotificationAgent(string exchangeServerUserName,
            string exchangeServerPassword,
            string exchangeServerDomain,
            string DefaultFromEmail)
        {
            ExchangeService = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            ExchangeService.Credentials = new WebCredentials(exchangeServerUserName, exchangeServerPassword, exchangeServerDomain);
            ExchangeService.AutodiscoverUrl(DefaultFromEmail);            
        }

        public async override Task<IEnumerable<NotificationResponse>> Send(IEnumerable<Email> notifications)
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
                resp.Add(new NotificationResponse
                {
                    DeliveryStatus = DeliveryStatus.Bounced,
                    NotificationStatus = NotificationStatus.Failed,
                    Exception = ex
                });
            }            

            return resp;
        }

        public override NotificationResponse Send(Email notification)
        {
            try
            {
                var emailMessage = this.GetEmailMessage(notification);

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
               return new NotificationResponse
                {
                    DeliveryStatus = DeliveryStatus.Bounced,
                    NotificationStatus = NotificationStatus.Failed,
                    Exception = ex
                };
            }
        }

        public override async Task<NotificationResponse> SendAsync(Email notification)
        {
            try
            {
                var emailMessage = this.GetEmailMessage(notification);

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
               return new NotificationResponse
                {
                    DeliveryStatus = DeliveryStatus.Bounced,
                    NotificationStatus = NotificationStatus.Failed,
                    Exception = ex
                };
            }
        }

        private EmailMessage GetEmailMessage(Email email)
        {
            try
            {
                EmailMessage emailMessage = new EmailMessage(this.ExchangeService);
                emailMessage.Subject = email.Subject;
                emailMessage.Body = new MessageBody(email.Content);
                if (email.Priority == MailPriority.High)
                {
                    emailMessage.Importance = Importance.High;
                }

                email.To.ForEach(to =>
                {
                    emailMessage.ToRecipients.Add(to);
                });

                if (email.CC != null)
                {
                    email.CC.ForEach(to =>
                    {
                        emailMessage.CcRecipients.Add(to);
                    });
                }

                if (email.BCC != null)
                {
                    email.BCC.ForEach(to =>
                    {
                        emailMessage.BccRecipients.Add(to);
                    });
                }

                if (email.Attachments != null)
                {
                    email.Attachments.ForEach(at =>
                    {
                        emailMessage.Attachments.AddFileAttachment(at.Name, at.Content);
                    });
                }

                emailMessage.From = email.FromEmail;

                return emailMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
