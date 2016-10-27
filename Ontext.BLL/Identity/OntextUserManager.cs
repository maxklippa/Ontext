using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Ontext.BLL.Identity.Services;
using Ontext.DAL.Identity;
using Ontext.Server.Core.Settings;
using System;
using System.Linq;
using SmsService = Ontext.BLL.Services.SmsService;

namespace Ontext.BLL.Identity
{
    public sealed class OntextUserManager : UserManager<OntextUser, Guid>
    {
        public OntextUserManager(IUserStore<OntextUser, Guid> store)
            : base(store)
        {
            UserValidator = new UserValidator<OntextUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true
            };

            RegisterTwoFactorProvider(OntextSettings.PhoneTwoFactorProvider, new PhoneNumberTokenProvider<OntextUser, Guid>
            {
                MessageFormat = "Your security code is: {0}"
            });
            RegisterTwoFactorProvider(OntextSettings.EmailTwoFactorProvider, new EmailTokenProvider<OntextUser, Guid>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });

            EmailService = new EmailService();
            SmsService = new Services.SmsService();
        }

        public OntextUser FindByPhoneNumber(string phoneNumber)
        {
            return Users.Include(u => u.Contacts).Include(u => u.Phones).FirstOrDefault(u => u.Phones.Any(p => p.Number == phoneNumber));
        }

        public void SendSms(string phoneNumber, string message)
        {
            var identityMessage = new IdentityMessage
            {
                Destination = phoneNumber,
                Body = message
            };

            this.SmsService.SendAsync(identityMessage);
        }

        public void SendEmail(string email, string subject, string message)
        {
            var identityMessage = new IdentityMessage
            {
                Destination = email,
                Body = message,
                Subject = subject
            };

            this.EmailService.Send(identityMessage);
        }
    }
}