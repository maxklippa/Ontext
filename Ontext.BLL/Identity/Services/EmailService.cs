using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ontext.Server.Core.Settings;

namespace Ontext.BLL.Identity.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Credentials:
            var credentialUserName = OntextSettings.SendGridUserName;
            var sentFrom = OntextSettings.SendGridEmail;
            var pwd = OntextSettings.SendGridPassword;

            // Confirm the client
            var client = new System.Net.Mail.SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            var credentials = new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:
            return client.SendMailAsync(mail);
        }
    }
}