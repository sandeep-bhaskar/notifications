using Notification.Core;
using Notification.Concerns;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Notification.Core.Providers;
using System.Collections.Generic;

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

        public override Task<IEnumerable<NotificationResponse>> Send(IEnumerable<T> notifications)
        {
            //try
            //{
            //    var message = this.GetMailMessage(mail);
            //    this.SMTPClient.Send(message);
            //    return new NotificationResponse
            //    {
            //        DeliveryStatus = DeliveryStatus.Delivered,
            //        NotificationStatus = NotificationStatus.Sent
            //    };
            //}
            //catch (Exception ex)
            //{

            //}

            //return new EmailResponse
            //{
            //    DeliveryStatus = DeliveryStatus.Bounced,
            //    EmailStatus = EmailStatus.Failed
            //};

            throw new NotImplementedException();
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
            message.Subject = mail.Subject;
            message.Body = mail.Content;
            return message;
        }
    }
}
