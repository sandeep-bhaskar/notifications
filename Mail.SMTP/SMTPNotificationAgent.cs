using Notification.Core;
using Notification.Concerns;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Notification.Core.Providers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Notification.SMTP
{
    public class SMTPNotificationAgent<T> : NotificationAgent<T> where T : Notification.Concerns.Notification
    {
        private SmtpClient SMTPClient { get; set; }

        private NotificationConfiguration Config { get; set; }

        public SMTPNotificationAgent(NotificationConfiguration config)
        {
            this.SMTPClient = new SmtpClient(config.Host, config.Port);
            this.SMTPClient.EnableSsl = true;
            this.SMTPClient.UseDefaultCredentials = false;
            this.SMTPClient.Credentials = new System.Net.NetworkCredential(config.UserName, config.Password);
            this.Config = config;
        }


        public override NotificationResponse Send(T notification)
        {
            try
            {
                var message = this.GetMailMessage(notification as Email);
                this.SMTPClient.SendMailAsync(message);
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
                var message = this.GetMailMessage(notification as Email);
                await this.SMTPClient.SendMailAsync(message);
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

        private MailMessage GetMailMessage(Email mail)
        {
            MailMessage message = new MailMessage();
            mail.FromEmail = string.IsNullOrEmpty(mail.FromEmail) ? this.Config.UserName : mail.FromEmail;
            message.From = new MailAddress(mail.FromEmail);
            mail.To.ForEach(to =>
            {
                message.To.Add(to);
            });

            if (mail.Attachments != null)
            {
                mail.Attachments.ForEach(at =>
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(at.Content), at.Name));
                });
            }

            if (mail.CC != null)
            {
                mail.CC.ForEach(cc =>
                {
                    message.CC.Add(cc);
                });
            }

            if (mail.BCC != null)
            {
                mail.BCC.ForEach(Bcc =>
                {
                    message.Bcc.Add(Bcc);
                });
            }

            if (mail.Priority == Concerns.MailPriority.High)
            {
                message.Priority = System.Net.Mail.MailPriority.High;
            }

            message.Subject = mail.Subject;
            message.Body = mail.Content;
            return message;
        }
    }
}
